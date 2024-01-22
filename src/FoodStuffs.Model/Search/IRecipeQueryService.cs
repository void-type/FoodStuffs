using FoodStuffs.Model.Events.Recipes;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search;

public interface IRecipeQueryService
{
    IItemSet<SearchRecipesResponse> Search(SearchRecipesRequest request);
}
