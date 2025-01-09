using FoodStuffs.Model.Search.Recipes;
using FoodStuffs.Model.Search.Recipes.Models;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

public class SuggestRecipesHandler : EventHandlerSyncAbstract<SuggestRecipesRequest, IItemSet<SuggestRecipesResultItem>>
{
    private readonly IRecipeQueryService _query;

    public SuggestRecipesHandler(IRecipeQueryService query)
    {
        _query = query;
    }

    protected override IResult<IItemSet<SuggestRecipesResultItem>> HandleSync(SuggestRecipesRequest request)
    {
        return Ok(_query.Suggest(request));
    }
}
