using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.GroceryAisles.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.GroceryAisles;

public class DeleteGroceryAisleHandler : CustomEventHandlerAbstract<DeleteGroceryAisleRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;

    public DeleteGroceryAisleHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteGroceryAisleRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.GroceryAisles
            .TagWith(GetTag())
            .Include(c => c.GroceryItems)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new GroceryAisleNotFoundFailure())
            .TeeOnSuccessAsync(async c =>
            {
                _data.GroceryAisles.Remove(c);

                await _data.SaveChangesAsync(cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Grocery aisle deleted.", r.Id));
    }
}
