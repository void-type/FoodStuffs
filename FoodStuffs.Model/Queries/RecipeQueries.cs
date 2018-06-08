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
                recipe.CategoryRecipe.Any(cr => cr.Category.Name.ToUpper().Contains(searchTerm))));
        }

        public static IQueryable<Recipe> SearchNames(this IQueryable<Recipe> recipes, string nameSearch)
        {
            return recipes.Where(recipe => recipe.Name.ToUpper()
                .Contains(nameSearch.ToUpper().Trim()));
        }

        public static IEnumerable<IRecipeViewModel> ToViewModels(this IEnumerable<Recipe> recipes)
        {
            return recipes.Select(r => r.ToViewModel());
        }

        public static IRecipeViewModel ToViewModel(this Recipe recipe)
        {
            return new RecipeViewModel
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Directions = recipe.Directions,
                Ingredients = recipe.Ingredients,
                CookTimeMinutes = recipe.CookTimeMinutes,
                PrepTimeMinutes = recipe.PrepTimeMinutes,
                CreatedBy = $"{recipe.CreatedByUser?.FirstName} {recipe.CreatedByUser?.LastName}",
                CreatedOnUtc = recipe.CreatedOnUtc,
                ModifiedBy = $"{recipe.ModifiedByUser?.FirstName} {recipe.ModifiedByUser?.LastName}",
                ModifiedOnUtc = recipe.ModifiedOnUtc,
                Categories = recipe.CategoryRecipe.Select(cr => cr.Category.Name)
            };
        }
    }
}