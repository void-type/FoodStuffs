using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.GroceryItems.Models;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.GroceryItems;

public class SuggestGroceryItemsHandler : EventHandlerSyncAbstract<SuggestGroceryItemsRequest, IItemSet<SuggestGroceryItemsResultItem>>
{
    private readonly IGroceryItemQueryService _query;

    public SuggestGroceryItemsHandler(IGroceryItemQueryService query)
    {
        _query = query;
    }

    protected override IResult<IItemSet<SuggestGroceryItemsResultItem>> HandleSync(SuggestGroceryItemsRequest request)
    {
        return Ok(_query.Suggest(request));
    }
}
