using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.PantryLocations.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.PantryLocations;

public class DeletePantryLocationHandler : CustomEventHandlerAbstract<DeletePantryLocationRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;

    public DeletePantryLocationHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(DeletePantryLocationRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.PantryLocations
            .TagWith(GetTag())
            .Include(c => c.GroceryItems)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new PantryLocationNotFoundFailure())
            .TeeOnSuccessAsync(async c =>
            {
                _data.PantryLocations.Remove(c);

                await _data.SaveChangesAsync(cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Storage location deleted.", r.Id));
    }
}
