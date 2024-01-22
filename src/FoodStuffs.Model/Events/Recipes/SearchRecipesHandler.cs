using FoodStuffs.Model.Search;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesHandler : EventHandlerSyncAbstract<SearchRecipesRequest, IItemSet<SearchRecipesResponse>>
{
    private readonly IRecipeQueryService _queryService;

    public SearchRecipesHandler(IRecipeQueryService queryService)
    {
        _queryService = queryService;
    }

    protected override IResult<IItemSet<SearchRecipesResponse>> HandleSync(SearchRecipesRequest request)
    {
        return Ok(_queryService.Search(request));
    }
}
