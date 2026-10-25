using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.GroceryAisles.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.GroceryAisles;

public class DeleteGroceryAisleHandler(FoodStuffsContext data, ISearchIndexService searchIndex) : CustomEventHandlerAbstract<DeleteGroceryAisleRequest, EntityMessage<int>>
{
    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteGroceryAisleRequest request, CancellationToken cancellationToken = default)
    {
        return await data.GroceryAisles
            .TagWith(GetTag())
            .Include(c => c.GroceryItems)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new GroceryAisleNotFoundFailure())
            .TeeOnSuccessAsync(async c =>
            {
                var groceryItemIds = c.GroceryItems.Select(gi => gi.Id).ToList();

                data.GroceryAisles.Remove(c);

                await data.SaveChangesAsync(cancellationToken);

                searchIndex.EnqueueUpdate(SearchIndex.GroceryItems, groceryItemIds);
            })
            .SelectAsync(r => EntityMessage.Create("Grocery aisle deleted.", r.Id));
    }
}
