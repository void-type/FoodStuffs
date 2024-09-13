using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class SaveShoppingItemResponseLogger : EntityMessageEventLogger<SaveShoppingItemRequest, int>
{
    public SaveShoppingItemResponseLogger(ILogger<SaveShoppingItemResponseLogger> logger) : base(logger) { }
}
