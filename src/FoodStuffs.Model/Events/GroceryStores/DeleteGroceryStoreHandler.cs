using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.GroceryStores.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.GroceryStores;

public class DeleteGroceryStoreHandler(FoodStuffsContext data, ISearchIndexService searchIndex) : CustomEventHandlerAbstract<DeleteGroceryStoreRequest, EntityMessage<int>>
{
    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteGroceryStoreRequest request, CancellationToken cancellationToken = default)
    {
        return await data.GroceryStores
            .TagWith(GetTag())
            .Include(c => c.GroceryItems)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new GroceryStoreNotFoundFailure())
            .TeeOnSuccessAsync(async c =>
            {
                data.GroceryStores.Remove(c);

                await data.SaveChangesAsync(cancellationToken);

                searchIndex.EnqueueUpdate(SearchIndex.GroceryItems, c.GroceryItems.Select(gi => gi.Id));
            })
            .SelectAsync(r => EntityMessage.Create("Grocery store deleted.", r.Id));
    }
}
