using FoodStuffs.Model.Search;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesHandler : EventHandlerSyncAbstract<SearchRecipesRequest, IItemSet<SearchRecipesResponse>>
{
    private readonly IRecipeQueryService _query;

    public SearchRecipesHandler(IRecipeQueryService query)
    {
        _query = query;
    }

    protected override IResult<IItemSet<SearchRecipesResponse>> HandleSync(SearchRecipesRequest request)
    {
        return Ok(_query.Search(request));
    }
}
