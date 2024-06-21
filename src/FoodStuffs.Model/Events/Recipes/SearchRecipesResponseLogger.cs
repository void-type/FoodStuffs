using FoodStuffs.Model.Search.Recipes.Models;
using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesResponseLogger : FallibleEventLoggerAbstract<SearchRecipesRequest, SearchRecipesResponse>
{
    public SearchRecipesResponseLogger(ILogger<SearchRecipesResponseLogger> logger) : base(logger) { }
}
