using FoodStuffs.Model.Search.Recipes.Models;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesPipeline : EventPipelineAbstract<SearchRecipesRequest, SearchRecipesResponse>
{
    public SearchRecipesPipeline(SearchRecipesHandler handler, SearchRecipesRequestLogger requestLogger, SearchRecipesResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
