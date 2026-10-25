using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.StorageLocations.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.StorageLocations;

public class DeleteStorageLocationHandler(FoodStuffsContext data, ISearchIndexService searchIndex) : CustomEventHandlerAbstract<DeleteStorageLocationRequest, EntityMessage<int>>
{
    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteStorageLocationRequest request, CancellationToken cancellationToken = default)
    {
        return await data.StorageLocations
            .TagWith(GetTag())
            .Include(c => c.GroceryItems)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new StorageLocationNotFoundFailure())
            .TeeOnSuccessAsync(async c =>
            {
                data.StorageLocations.Remove(c);

                await data.SaveChangesAsync(cancellationToken);

                searchIndex.EnqueueUpdate(SearchIndex.GroceryItems, c.GroceryItems.Select(gi => gi.Id));
            })
            .SelectAsync(r => EntityMessage.Create("Storage location deleted.", r.Id));
    }
}
