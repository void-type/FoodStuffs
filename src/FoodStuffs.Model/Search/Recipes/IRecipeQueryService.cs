using FoodStuffs.Model.Search.Recipes.Models;

namespace FoodStuffs.Model.Search.Recipes;

public interface IRecipeQueryService
{
    SearchRecipesResponse Search(SearchRecipesRequest request);
}
