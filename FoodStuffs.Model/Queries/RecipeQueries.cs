using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Queries
{
    public static class RecipeQueries
    {
        public static IRecipe GetById(this IQueryable<IRecipe> recipes, int id)
        {
            return recipes.SingleOrDefault(r => r.Id == id);
        }

        public static IQueryable<IRecipe> SearchCategory(this IQueryable<IRecipe> recipes, string categorySearch)
        {
            categorySearch = categorySearch.ToUpper().Trim();

            // TODO: This makes the search work for EF. Find out why.
            var contains = recipes.ToList();

            return recipes.Where(recipe => recipe.CategoryRecipes
                .Any(cr => cr.Category.Name.ToUpper().Contains(categorySearch)));
        }

        public static IQueryable<IRecipe> SearchNames(this IQueryable<IRecipe> recipes, string nameSearch)
        {
            return recipes.Where(recipe => recipe.Name.ToUpper()
                .Contains(nameSearch.ToUpper().Trim()));
        }

        public static IEnumerable<IRecipeViewModel> ToViewModel(this IEnumerable<IRecipe> recipes)
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