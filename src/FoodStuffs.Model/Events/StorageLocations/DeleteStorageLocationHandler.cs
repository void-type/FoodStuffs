using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.StorageLocations.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.StorageLocations;

public class DeleteStorageLocationHandler : CustomEventHandlerAbstract<DeleteStorageLocationRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly ISearchIndexService _searchIndex;

    public DeleteStorageLocationHandler(FoodStuffsContext data, ISearchIndexService searchIndex)
    {
        _data = data;
        _searchIndex = searchIndex;
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

                _searchIndex.EnqueueUpdate(SearchIndex.GroceryItems, c.GroceryItems.Select(gi => gi.Id));
            })
            .SelectAsync(r => EntityMessage.Create("Storage location deleted.", r.Id));
    }
}
