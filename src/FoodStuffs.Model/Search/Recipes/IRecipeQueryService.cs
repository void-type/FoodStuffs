using FoodStuffs.Model.Search.Recipes.Models;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search.Recipes;

public interface IRecipeQueryService
{
    /// <summary>
    /// Search recipes with facets and pagination.
    /// </summary>
    SearchRecipesResponse Search(SearchRecipesRequest request);

    /// <summary>
    /// Suggest recipes for autocompletion.
    /// </summary>
    IItemSet<SuggestRecipesResultItem> Suggest(SuggestRecipesRequest request);
}
