using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.StorageLocations.Models;
using FoodStuffs.Model.Search.GroceryItems;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.StorageLocations;

public class DeleteStorageLocationHandler : CustomEventHandlerAbstract<DeleteStorageLocationRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly IGroceryItemIndexService _groceryItemIndex;

    public DeleteStorageLocationHandler(FoodStuffsContext data, IGroceryItemIndexService groceryItemIndex)
    {
        _data = data;
        _groceryItemIndex = groceryItemIndex;
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

                await _groceryItemIndex.AddOrUpdateAsync(c.GroceryItems.Select(gi => gi.Id), cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Storage location deleted.", r.Id));
    }
}
