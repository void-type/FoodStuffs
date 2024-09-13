using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class SaveShoppingItemPipeline : EventPipelineAbstract<SaveShoppingItemRequest, EntityMessage<int>>
{
    public SaveShoppingItemPipeline(SaveShoppingItemHandler handler, SaveShoppingItemRequestLogger requestLogger, SaveShoppingItemRequestValidator validator, SaveShoppingItemResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddRequestValidator(validator)
            .AddPostProcessor(responseLogger);
    }
}
