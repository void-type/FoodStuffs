using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryStores.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.GroceryStores;

public class SaveGroceryStoreHandler : CustomEventHandlerAbstract<SaveGroceryStoreRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SaveGroceryStoreRequestValidator _validator;
    private readonly ISearchIndexService _searchIndex;

    public SaveGroceryStoreHandler(FoodStuffsContext data, SaveGroceryStoreRequestValidator validator, ISearchIndexService searchIndex)
    {
        _data = data;
        _validator = validator;
        _searchIndex = searchIndex;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveGroceryStoreRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveGroceryStoreRequest request, CancellationToken cancellationToken)
    {
        var requestedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.Name).Trim();

        var byId = new GroceryStoresWithAllRelatedSpecification(request.Id);

        var maybeGroceryStore = await _data.GroceryStores
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        // Check for conflicting items by name
        var byName = new GroceryStoresSpecification(requestedName);

        var conflictingGroceryStore = await _data.GroceryStores
            .TagWith(GetTag(byName))
            .AsSplitQuery()
            .ApplyEfSpecification(byName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (conflictingGroceryStore is not null && conflictingGroceryStore.Id != request.Id)
        {
            return Fail(new Failure("Grocery store name already exists.", "name"));
        }

        var groceryStoreToEdit = maybeGroceryStore.Unwrap(() => new GroceryStore());

        var relatedGroceryItemIds = groceryStoreToEdit.GroceryItems.Select(gi => gi.Id).ToHashSet();

        Transfer(requestedName, groceryStoreToEdit);

        if (maybeGroceryStore.HasValue)
        {
            _data.GroceryStores.Update(groceryStoreToEdit);
        }
        else
        {
            _data.GroceryStores.Add(groceryStoreToEdit);
        }

        await _data.SaveChangesAsync(cancellationToken);

        relatedGroceryItemIds.UnionWith(groceryStoreToEdit.GroceryItems.Select(gi => gi.Id));

        _searchIndex.EnqueueUpdate(SearchIndex.GroceryItems, relatedGroceryItemIds);

        return Ok(EntityMessage.Create($"Grocery Store {(maybeGroceryStore.HasValue ? "updated" : "added")}.", groceryStoreToEdit.Id));
    }

    private static void Transfer(string formattedName, GroceryStore groceryStore)
    {
        groceryStore.Name = formattedName;
    }
}
