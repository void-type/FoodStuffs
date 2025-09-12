using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryItems.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.GroceryItems;

public class SaveGroceryItemInventoryHandler : CustomEventHandlerAbstract<SaveGroceryItemInventoryRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SaveGroceryItemInventoryRequestValidator _validator;
    private readonly ISearchIndexService _searchIndex;

    public SaveGroceryItemInventoryHandler(FoodStuffsContext data, SaveGroceryItemInventoryRequestValidator validator, ISearchIndexService searchIndex)
    {
        _data = data;
        _validator = validator;
        _searchIndex = searchIndex;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveGroceryItemInventoryRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveGroceryItemInventoryRequest request, CancellationToken cancellationToken)
    {
        var byId = new GroceryItemsWithAllRelatedSpecification(request.Id);

        var maybeGroceryItem = await _data.GroceryItems
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        if (!maybeGroceryItem.HasValue)
        {
            return Fail(new GroceryItemNotFoundFailure());
        }

        var groceryItemToEdit = maybeGroceryItem.Value;

        Transfer(request, groceryItemToEdit);

        _data.GroceryItems.Update(groceryItemToEdit);

        await _data.SaveChangesAsync(cancellationToken);

        await _searchIndex.AddOrUpdateAsync(SearchIndex.GroceryItems, groceryItemToEdit.Id, cancellationToken);

        return Ok(EntityMessage.Create($"Grocery item {(maybeGroceryItem.HasValue ? "updated" : "added")}.", groceryItemToEdit.Id));
    }

    private static void Transfer(SaveGroceryItemInventoryRequest request, GroceryItem groceryItem)
    {
        groceryItem.InventoryQuantity = request.InventoryQuantity;
    }
}
