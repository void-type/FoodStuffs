using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.GroceryStores.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.GroceryStores;

public class DeleteGroceryStoreHandler : CustomEventHandlerAbstract<DeleteGroceryStoreRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly ISearchIndexService _searchIndex;

    public DeleteGroceryStoreHandler(FoodStuffsContext data, ISearchIndexService searchIndex)
    {
        _data = data;
        _searchIndex = searchIndex;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteGroceryStoreRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.GroceryStores
            .TagWith(GetTag())
            .Include(c => c.GroceryItems)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new GroceryStoreNotFoundFailure())
            .TeeOnSuccessAsync(async c =>
            {
                _data.GroceryStores.Remove(c);

                await _data.SaveChangesAsync(cancellationToken);

                _searchIndex.EnqueueUpdate(SearchIndex.GroceryItems, c.GroceryItems.Select(gi => gi.Id));
            })
            .SelectAsync(r => EntityMessage.Create("Grocery store deleted.", r.Id));
    }
}
