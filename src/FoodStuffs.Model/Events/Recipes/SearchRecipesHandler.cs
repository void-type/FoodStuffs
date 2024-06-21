using FoodStuffs.Model.Search.Recipes;
using FoodStuffs.Model.Search.Recipes.Models;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesHandler : EventHandlerSyncAbstract<SearchRecipesRequest, SearchRecipesResponse>
{
    private readonly IRecipeQueryService _query;

    public SearchRecipesHandler(IRecipeQueryService query)
    {
        _query = query;
    }

    protected override IResult<SearchRecipesResponse> HandleSync(SearchRecipesRequest request)
    {
        return Ok(_query.Search(request));
    }
}
