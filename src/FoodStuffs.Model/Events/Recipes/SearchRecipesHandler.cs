using FoodStuffs.Model.Search.Recipes;
using FoodStuffs.Model.Search.Recipes.Models;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesHandler(IRecipeQueryService query) : EventHandlerSyncAbstract<SearchRecipesRequest, SearchRecipesResponse>
{
    protected override IResult<SearchRecipesResponse> HandleSync(SearchRecipesRequest request)
    {
        return Ok(query.Search(request));
    }
}
