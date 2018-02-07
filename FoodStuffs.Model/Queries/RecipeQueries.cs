using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Queries
{
    public static class RecipeQueries
    {
        public static Recipe GetById(this IQueryable<Recipe> recipes, int id)
        {
            return recipes.SingleOrDefault(r => r.Id == id);
        }

        public static IQueryable<Recipe> SearchCategory(this IQueryable<Recipe> recipes, string categorySearch)
        {
            var categorySearches = categorySearch.ToUpper().Trim().Split(' ');

            return recipes.Where(recipe => categorySearches.All(searchTerm =>
                recipe.CategoryRecipes.Any(cr => cr.Category.Name.ToUpper().Contains(searchTerm))));
        }

        public static IQueryable<Recipe> SearchNames(this IQueryable<Recipe> recipes, string nameSearch)
        {
            return recipes.Where(recipe => recipe.Name.ToUpper()
                .Contains(nameSearch.ToUpper().Trim()));
        }

        public static IEnumerable<IRecipeViewModel> ToViewModel(this IEnumerable<Recipe> recipes)
        {
            return recipes.Select(r => new RecipeViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Directions = r.Directions,
                Ingredients = r.Ingredients,
                CookTimeMinutes = r.CookTimeMinutes,
                PrepTimeMinutes = r.PrepTimeMinutes,
                CreatedBy = $"{r.CreatedByUser.FirstName} {r.CreatedByUser.LastName}",
                CreatedOn = r.CreatedOn,
                ModifiedBy = $"{r.ModifiedByUser.FirstName} {r.ModifiedByUser.LastName}",
                ModifiedOn = r.ModifiedOn,
                Categories = r.CategoryRecipes.Select(cr => cr.Category.Name)
            });
        }
    }
}