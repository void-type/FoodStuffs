using FoodStuffs.Model.Data.Models;
using System.Linq;

namespace FoodStuffs.Model.Queries
{
    public static class CategoryRecipeQueries
    {
        public static ICategoryRecipe GetById(this IQueryable<ICategoryRecipe> categoryRecipes, int recipeId,
            int categoryId)
        {
            return categoryRecipes.SingleOrDefault(cr => cr.RecipeId == recipeId && cr.CategoryId == categoryId);
        }
    }
}