using FoodStuffs.Model.Search;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesPipeline : EventPipelineAbstract<RecipeSearchRequest, RecipeSearchResponse>
{
    public SearchRecipesPipeline(SearchRecipesHandler handler, SearchRecipesRequestLogger requestLogger, SearchRecipesResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
