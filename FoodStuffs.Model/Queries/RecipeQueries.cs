using FoodStuffs.Model.Interfaces.Services.Data.Models;
using FoodStuffs.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Queries
{
    public static class RecipeQueries
    {
        public static IQueryable<IRecipe> ForCategory(this IQueryable<IRecipe> recipes, int categoryId)
        {
            return recipes.Where(recipe => recipe.CategoryRecipe
                .Select(cr => cr.CategoryId)
                .Contains(categoryId));
        }

        public static IRecipe GetById(this IQueryable<IRecipe> recipes, int id)
        {
            return recipes.SingleOrDefault(r => r.Id == id);
        }

        public static IQueryable<IRecipe> SearchNames(this IQueryable<IRecipe> recipes, string nameSearch)
        {
            return recipes.Where(recipe => recipe.Name.ToUpper().Contains(nameSearch.ToUpper().Trim()));
        }

        public static IEnumerable<RecipeViewModel> ToViewModel(this IEnumerable<IRecipe> recipes)
        {
            return recipes.Select(r => new RecipeViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Directions = r.Directions,
                Ingredients = r.Ingredients,
                CookTimeMinutes = r.CookTimeMinutes,
                PrepTimeMinutes = r.PrepTimeMinutes,
                CreatedByUserId = r.CreatedByUserId,
                CreatedOn = r.CreatedOn,
                ModifiedByUserId = r.ModifiedByUserId,
                ModifiedOn = r.ModifiedOn,
                Categories = r.CategoryRecipe.Select(cr => cr.Category.Name)
            });
        }
    }
}