using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class DeleteShoppingItemPipeline : EventPipelineAbstract<DeleteShoppingItemRequest, EntityMessage<int>>
{
    public DeleteShoppingItemPipeline(DeleteShoppingItemHandler handler, DeleteShoppingItemRequestLogger requestLogger, DeleteShoppingItemResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
