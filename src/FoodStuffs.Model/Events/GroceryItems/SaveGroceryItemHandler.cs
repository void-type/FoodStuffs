using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryItems.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.GroceryItems;

public class SaveGroceryItemHandler : CustomEventHandlerAbstract<SaveGroceryItemRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SaveGroceryItemRequestValidator _validator;
    private readonly ISearchIndexService _searchIndex;

    public SaveGroceryItemHandler(FoodStuffsContext data, SaveGroceryItemRequestValidator validator, ISearchIndexService searchIndex)
    {
        _data = data;
        _validator = validator;
        _searchIndex = searchIndex;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveGroceryItemRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveGroceryItemRequest request, CancellationToken cancellationToken)
    {
        var requestedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.Name).Trim();

        var byId = new GroceryItemsWithAllRelatedSpecification(request.Id);

        var maybeGroceryItem = await _data.GroceryItems
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        // Check for conflicting items by name
        var byName = new GroceryItemsSpecification(requestedName);

        var conflictingGroceryItem = await _data.GroceryItems
            .TagWith(GetTag(byName))
            .AsSplitQuery()
            .ApplyEfSpecification(byName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (conflictingGroceryItem is not null && conflictingGroceryItem.Id != request.Id)
        {
            return Fail(new Failure("Grocery item name already exists.", "name"));
        }

        var groceryItemToEdit = maybeGroceryItem.Unwrap(() => new GroceryItem());

        var relatedRecipeIds = groceryItemToEdit.Recipes.Select(r => r.Id).ToHashSet();

        Transfer(requestedName, request, groceryItemToEdit);

        await ManageStorageLocationsAsync(request, groceryItemToEdit, cancellationToken);

        if (request.GroceryAisleId is not null)
        {
            var groceryAisle = await _data.GroceryAisles
                .TagWith(GetTag())
                .FirstOrDefaultAsync(gd => gd.Id == request.GroceryAisleId, cancellationToken);

            if (groceryAisle is null)
            {
                return Fail(new GroceryAisleNotFoundFailure());
            }

            groceryItemToEdit.GroceryAisle = groceryAisle;
        }

        if (maybeGroceryItem.HasValue)
        {
            _data.GroceryItems.Update(groceryItemToEdit);
        }
        else
        {
            _data.GroceryItems.Add(groceryItemToEdit);
        }

        await _data.SaveChangesAsync(cancellationToken);

        relatedRecipeIds.UnionWith(groceryItemToEdit.Recipes.Select(r => r.Id));

        await _searchIndex.AddOrUpdateAsync(SearchIndex.Recipes, relatedRecipeIds, cancellationToken);

        await _searchIndex.AddOrUpdateAsync(SearchIndex.GroceryItems, groceryItemToEdit.Id, cancellationToken);

        return Ok(EntityMessage.Create($"Grocery Item {(maybeGroceryItem.HasValue ? "updated" : "added")}.", groceryItemToEdit.Id));
    }

    private static void Transfer(string formattedName, SaveGroceryItemRequest request, GroceryItem groceryItem)
    {
        groceryItem.Name = formattedName;
        groceryItem.InventoryQuantity = request.InventoryQuantity;
    }

    private async Task ManageStorageLocationsAsync(SaveGroceryItemRequest request, GroceryItem groceryItem, CancellationToken cancellationToken)
    {
        var requestedNames = request.StorageLocations
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x).Trim())
            .ToArray();

        // Remove extra storage locations.
        groceryItem.StorageLocations.RemoveAll(x => !requestedNames.Contains(x.Name, StringComparer.OrdinalIgnoreCase));

        var missingNames = requestedNames
            .Where(n => !groceryItem.StorageLocations.Select(x => x.Name).Contains(n, StringComparer.OrdinalIgnoreCase))
            .ToList();

        // Find missing storage locations that already exist.
        var existingStorageLocations = await _data.StorageLocations
            .TagWith(GetTag())
            .Where(x => missingNames.Contains(x.Name))
            .OrderBy(x => x.Id)
            // In case there are duplicates, add only the first.
            .GroupBy(x => x.Name)
            .Select(g => g.First())
            .ToListAsync(cancellationToken);

        groceryItem.StorageLocations.AddRange(existingStorageLocations);

        // Create missing StorageLocations that don't exist.
        var createdStorageLocations = missingNames
            .Where(x => !groceryItem.StorageLocations.Select(x => x.Name).Contains(x, StringComparer.OrdinalIgnoreCase))
            .Select(x => new StorageLocation { Name = x });

        groceryItem.StorageLocations.AddRange(createdStorageLocations);
    }
}
