using VoidCore.Model.Events;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class ListShoppingItemsPipeline : EventPipelineAbstract<ListShoppingItemsRequest, IItemSet<ListShoppingItemsResponse>>
{
    public ListShoppingItemsPipeline(ListShoppingItemsHandler handler, ListShoppingItemsRequestLogger requestLogger, ListShoppingItemsResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
