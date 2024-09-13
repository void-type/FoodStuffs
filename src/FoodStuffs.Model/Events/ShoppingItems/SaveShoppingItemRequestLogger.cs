using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class SaveShoppingItemRequestLogger : RequestLoggerAbstract<SaveShoppingItemRequest>
{
    public SaveShoppingItemRequestLogger(ILogger<SaveShoppingItemRequestLogger> logger) : base(logger) { }

    public override void Log(SaveShoppingItemRequest request)
    {
        Logger.LogInformation("Requested. ShoppingItemId: {ShoppingItemId} ShoppingItemName: {ShoppingItemName}",
            request.Id,
            request.Name);
    }
}
