using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class ListShoppingItemsResponseLogger : ItemSetEventLogger<ListShoppingItemsRequest, ListShoppingItemsResponse>
{
    public ListShoppingItemsResponseLogger(ILogger<ListShoppingItemsResponseLogger> logger) : base(logger) { }
}
