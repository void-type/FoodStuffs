using FoodStuffs.Model.Search.Recipes.Models;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search.Recipes;

public interface IRecipeQueryService
{
    SearchRecipesResponse Search(SearchRecipesRequest request);

    IItemSet<SuggestRecipesResultItem> Suggest(SuggestRecipesRequest request);
}
