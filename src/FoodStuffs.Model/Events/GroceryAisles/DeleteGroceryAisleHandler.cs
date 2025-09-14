using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.GroceryAisles.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.GroceryAisles;

public class DeleteGroceryAisleHandler : CustomEventHandlerAbstract<DeleteGroceryAisleRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly ISearchIndexService _searchIndex;

    public DeleteGroceryAisleHandler(FoodStuffsContext data, ISearchIndexService searchIndex)
    {
        _data = data;
        _searchIndex = searchIndex;
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
                var groceryItemIds = c.GroceryItems.Select(gi => gi.Id).ToList();

                _data.GroceryAisles.Remove(c);

                await _data.SaveChangesAsync(cancellationToken);

                await _searchIndex.AddOrUpdateAsync(SearchIndex.GroceryItems, groceryItemIds, cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Grocery aisle deleted.", r.Id));
    }
}
