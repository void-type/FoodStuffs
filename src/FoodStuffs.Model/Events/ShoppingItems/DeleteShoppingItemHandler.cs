using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.ShoppingItems.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class DeleteShoppingItemHandler : CustomEventHandlerAbstract<DeleteShoppingItemRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;

    public DeleteShoppingItemHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteShoppingItemRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.ShoppingItems
            .TagWith(GetTag())
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new ShoppingItemNotFoundFailure())
            .TeeOnSuccessAsync(async r =>
            {
                _data.ShoppingItems.Remove(r);
                await _data.SaveChangesAsync(cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Shopping item deleted.", r.Id));
    }
}
