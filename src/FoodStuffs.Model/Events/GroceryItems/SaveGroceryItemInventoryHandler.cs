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

public class SaveGroceryItemInventoryHandler(FoodStuffsContext data, SaveGroceryItemInventoryRequestValidator validator, ISearchIndexService searchIndex) : CustomEventHandlerAbstract<SaveGroceryItemInventoryRequest, EntityMessage<int>>
{
    public override async Task<IResult<EntityMessage<int>>> Handle(SaveGroceryItemInventoryRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveGroceryItemInventoryRequest request, CancellationToken cancellationToken)
    {
        var byId = new GroceryItemsWithAllRelatedSpecification(request.Id);

        var maybeGroceryItem = await data.GroceryItems
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

        data.GroceryItems.Update(groceryItemToEdit);

        await data.SaveChangesAsync(cancellationToken);

        await searchIndex.UpdateAsync(SearchIndex.GroceryItems, groceryItemToEdit.Id, cancellationToken);

        return Ok(EntityMessage.Create($"Grocery item {(maybeGroceryItem.HasValue ? "updated" : "added")}.", groceryItemToEdit.Id));
    }

    private static void Transfer(SaveGroceryItemInventoryRequest request, GroceryItem groceryItem)
    {
        groceryItem.InventoryQuantity = request.InventoryQuantity ?? 0;
    }
}
