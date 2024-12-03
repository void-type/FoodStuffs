using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class DeleteShoppingItemResponseLogger : EntityMessageEventLogger<DeleteShoppingItemRequest, int>
{
    public DeleteShoppingItemResponseLogger(ILogger<DeleteShoppingItemResponseLogger> logger) : base(logger) { }
}
