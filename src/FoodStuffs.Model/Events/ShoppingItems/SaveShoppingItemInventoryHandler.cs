using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.ShoppingItems.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class SaveShoppingItemInventoryHandler : CustomEventHandlerAbstract<SaveShoppingItemInventoryRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SaveShoppingItemInventoryRequestValidator _validator;

    public SaveShoppingItemInventoryHandler(FoodStuffsContext data, SaveShoppingItemInventoryRequestValidator validator)
    {
        _data = data;
        _validator = validator;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveShoppingItemInventoryRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveShoppingItemInventoryRequest request, CancellationToken cancellationToken)
    {
        var byId = new ShoppingItemsWithAllRelatedSpecification(request.Id);

        var maybeShoppingItem = await _data.ShoppingItems
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        if (!maybeShoppingItem.HasValue)
        {
            return Fail(new ShoppingItemNotFoundFailure());
        }

        var shoppingItemToEdit = maybeShoppingItem.Value;

        Transfer(request, shoppingItemToEdit);

        _data.ShoppingItems.Update(shoppingItemToEdit);

        await _data.SaveChangesAsync(cancellationToken);

        return Ok(EntityMessage.Create($"Shopping item {(maybeShoppingItem.HasValue ? "updated" : "added")}.", shoppingItemToEdit.Id));
    }

    private static void Transfer(SaveShoppingItemInventoryRequest request, ShoppingItem shoppingItem)
    {
        shoppingItem.InventoryQuantity = request.InventoryQuantity;
    }
}
