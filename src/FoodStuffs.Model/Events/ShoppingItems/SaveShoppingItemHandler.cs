using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.ShoppingItems.Models;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class SaveShoppingItemHandler : CustomEventHandlerAbstract<SaveShoppingItemRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SaveShoppingItemRequestValidator _validator;
    private readonly IRecipeIndexService _index;

    public SaveShoppingItemHandler(FoodStuffsContext data, SaveShoppingItemRequestValidator validator, IRecipeIndexService index)
    {
        _data = data;
        _validator = validator;
        _index = index;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveShoppingItemRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveShoppingItemRequest request, CancellationToken cancellationToken)
    {
        var requestedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.Name).Trim();

        var byId = new ShoppingItemsWithAllRelatedSpecification(request.Id);

        var maybeShoppingItem = await _data.ShoppingItems
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        // Check for conflicting items by name
        var byName = new ShoppingItemsSpecification(requestedName);

        var conflictingShoppingItem = await _data.ShoppingItems
            .TagWith(GetTag(byName))
            .AsSplitQuery()
            .ApplyEfSpecification(byName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (conflictingShoppingItem is not null && conflictingShoppingItem.Id != request.Id)
        {
            return Fail(new Failure("Grocery item name already exists.", "name"));
        }

        var shoppingItemToEdit = maybeShoppingItem.Unwrap(() => new ShoppingItem());

        Transfer(requestedName, request, shoppingItemToEdit);

        await ManagePantryLocationsAsync(request, shoppingItemToEdit, cancellationToken);

        if (request.GroceryDepartmentId is not null)
        {
            var groceryDepartment = await _data.GroceryDepartments
                .TagWith(GetTag())
                .FirstOrDefaultAsync(gd => gd.Id == request.GroceryDepartmentId, cancellationToken);

            if (groceryDepartment is null)
            {
                return Fail(new GroceryDepartmentNotFoundFailure());
            }

            shoppingItemToEdit.GroceryDepartment = groceryDepartment;
        }

        if (maybeShoppingItem.HasValue)
        {
            _data.ShoppingItems.Update(shoppingItemToEdit);
        }
        else
        {
            _data.ShoppingItems.Add(shoppingItemToEdit);
        }

        await _data.SaveChangesAsync(cancellationToken);

        await _index.AddOrUpdateAsync(shoppingItemToEdit.Recipes.Select(r => r.Id), cancellationToken);

        return Ok(EntityMessage.Create($"Grocery Item {(maybeShoppingItem.HasValue ? "updated" : "added")}.", shoppingItemToEdit.Id));
    }

    private static void Transfer(string formattedName, SaveShoppingItemRequest request, ShoppingItem shoppingItem)
    {
        shoppingItem.Name = formattedName;
        shoppingItem.InventoryQuantity = request.InventoryQuantity;
    }

    private async Task ManagePantryLocationsAsync(SaveShoppingItemRequest request, ShoppingItem shoppingItem, CancellationToken cancellationToken)
    {
        var requestedNames = request.PantryLocations
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x).Trim())
            .ToArray();

        // Remove extra storage locations.
        shoppingItem.PantryLocations.RemoveAll(x => !requestedNames.Contains(x.Name, StringComparer.OrdinalIgnoreCase));

        var missingNames = requestedNames
            .Where(n => !shoppingItem.PantryLocations.Select(x => x.Name).Contains(n, StringComparer.OrdinalIgnoreCase))
            .ToList();

        // Find missing storage locations that already exist.
        var existingPantryLocations = await _data.PantryLocations
            .TagWith(GetTag())
            .Where(x => missingNames.Contains(x.Name))
            .OrderBy(x => x.Id)
            // In case there are duplicates, add only the first.
            .GroupBy(x => x.Name)
            .Select(g => g.First())
            .ToListAsync(cancellationToken);

        shoppingItem.PantryLocations.AddRange(existingPantryLocations);

        // Create missing PantryLocations that don't exist.
        var createdPantryLocations = missingNames
            .Where(x => !shoppingItem.PantryLocations.Select(x => x.Name).Contains(x, StringComparer.OrdinalIgnoreCase))
            .Select(x => new PantryLocation { Name = x });

        shoppingItem.PantryLocations.AddRange(createdPantryLocations);
    }
}
