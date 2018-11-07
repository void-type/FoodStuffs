using FoodStuffs.Model.Data.Models;
using System.Linq;

namespace FoodStuffs.Model.Queries
{
    public static class CategoryRecipeQueries
    {
        public static IQueryable<CategoryRecipe> WhereForRecipe(this IQueryable<CategoryRecipe> collection, int recipeId)
        {
            return collection.Where(cr => cr.RecipeId == recipeId);
        }
    }
}
