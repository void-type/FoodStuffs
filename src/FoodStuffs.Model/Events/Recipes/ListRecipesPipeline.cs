using VoidCore.Model.Events;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

public class ListRecipesPipeline : EventPipelineAbstract<ListRecipesRequest, IItemSet<ListRecipesResponse>>
{
    public ListRecipesPipeline(ListRecipesHandler handler, ListRecipesRequestLogger requestLogger, ListRecipesResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
