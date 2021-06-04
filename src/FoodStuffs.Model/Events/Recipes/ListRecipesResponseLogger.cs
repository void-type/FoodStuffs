using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes
{
    public class ListRecipesResponseLogger : ItemSetEventLogger<ListRecipesRequest, ListRecipesResponse>
    {
        public ListRecipesResponseLogger(ILogger<ListRecipesResponseLogger> logger) : base(logger) { }
    }
}
