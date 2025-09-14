using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.GroceryItems.Models;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.GroceryItems;

public class SearchGroceryItemsHandler : EventHandlerSyncAbstract<SearchGroceryItemsRequest, SearchGroceryItemsResponse>
{
    private readonly IGroceryItemQueryService _query;

    public SearchGroceryItemsHandler(IGroceryItemQueryService query)
    {
        _query = query;
    }

    protected override IResult<SearchGroceryItemsResponse> HandleSync(SearchGroceryItemsRequest request)
    {
        return Ok(_query.Search(request));
    }
}
