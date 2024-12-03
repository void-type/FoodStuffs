using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class DeleteShoppingItemRequestLogger : RequestLoggerAbstract<DeleteShoppingItemRequest>
{
    public DeleteShoppingItemRequestLogger(ILogger<DeleteShoppingItemRequestLogger> logger) : base(logger) { }

    public override void Log(DeleteShoppingItemRequest request)
    {
        Logger.LogInformation("Requested. ShoppingItemId: {ShoppingItemId}",
            request.Id);
    }
}
