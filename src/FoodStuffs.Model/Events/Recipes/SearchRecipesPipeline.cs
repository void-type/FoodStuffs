using VoidCore.Model.Events;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesPipeline : EventPipelineAbstract<SearchRecipesRequest, IItemSet<SearchRecipesResponse>>
{
    public SearchRecipesPipeline(SearchRecipesHandler handler, SearchRecipesRequestLogger requestLogger, SearchRecipesResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
