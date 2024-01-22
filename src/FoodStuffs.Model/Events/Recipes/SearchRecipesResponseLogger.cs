using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesResponseLogger : ItemSetEventLogger<SearchRecipesRequest, SearchRecipesResponse>
{
    public SearchRecipesResponseLogger(ILogger<SearchRecipesResponseLogger> logger) : base(logger) { }
}
