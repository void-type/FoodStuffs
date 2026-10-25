using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.GroceryItems.Models;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.GroceryItems;

public class SearchGroceryItemsHandler(IGroceryItemQueryService query) : EventHandlerSyncAbstract<SearchGroceryItemsRequest, SearchGroceryItemsResponse>
{
    protected override IResult<SearchGroceryItemsResponse> HandleSync(SearchGroceryItemsRequest request)
    {
        return Ok(query.Search(request));
    }
}
