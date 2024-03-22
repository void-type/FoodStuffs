using FoodStuffs.Model.Search;
using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesResponseLogger : FallibleEventLoggerAbstract<RecipeSearchRequest, RecipeSearchResponse>
{
    public SearchRecipesResponseLogger(ILogger<SearchRecipesResponseLogger> logger) : base(logger) { }
}
