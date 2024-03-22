using FoodStuffs.Model.Search;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesHandler : EventHandlerSyncAbstract<RecipeSearchRequest, RecipeSearchResponse>
{
    private readonly IRecipeQueryService _query;

    public SearchRecipesHandler(IRecipeQueryService query)
    {
        _query = query;
    }

    protected override IResult<RecipeSearchResponse> HandleSync(RecipeSearchRequest request)
    {
        return Ok(_query.Search(request));
    }
}
