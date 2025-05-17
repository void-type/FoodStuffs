using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.StorageLocations.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.StorageLocations;

public class DeleteStorageLocationHandler : CustomEventHandlerAbstract<DeleteStorageLocationRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;

    public DeleteStorageLocationHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteStorageLocationRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.StorageLocations
            .TagWith(GetTag())
            .Include(c => c.GroceryItems)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new StorageLocationNotFoundFailure())
            .TeeOnSuccessAsync(async c =>
            {
                _data.StorageLocations.Remove(c);

                await _data.SaveChangesAsync(cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Storage location deleted.", r.Id));
    }
}
