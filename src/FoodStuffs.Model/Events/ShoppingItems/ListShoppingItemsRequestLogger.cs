using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class ListShoppingItemsRequestLogger : RequestLoggerAbstract<ListShoppingItemsRequest>
{
    public ListShoppingItemsRequestLogger(ILogger<ListShoppingItemsRequestLogger> logger) : base(logger) { }

    public override void Log(ListShoppingItemsRequest request)
    {
        Logger.LogInformation("Requested. NameSearch: {NameSearch} IsPagingEnabled: {IsPagingEnabled} Page: {Page} Take: {Take}",
            request.NameSearch,
            request.IsPagingEnabled,
            request.Page,
            request.Take);
    }
}
