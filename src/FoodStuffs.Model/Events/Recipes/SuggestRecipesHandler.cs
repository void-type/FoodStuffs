using FoodStuffs.Model.Search.Recipes;
using FoodStuffs.Model.Search.Recipes.Models;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

public class SuggestRecipesHandler(IRecipeQueryService query) : EventHandlerSyncAbstract<SuggestRecipesRequest, IItemSet<SuggestRecipesResultItem>>
{
    protected override IResult<IItemSet<SuggestRecipesResultItem>> HandleSync(SuggestRecipesRequest request)
    {
        return Ok(query.Suggest(request));
    }
}
