using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;
using static FoodStuffs.Model.DomainEvents.Recipes.ListRecipes;

namespace FoodStuffs.Model.Queries
{
    public static class RecipeQueries
    {
        public static Recipe GetById(this IQueryable<Recipe> recipes, int id)
        {
            return recipes.SingleOrDefault(r => r.Id == id);
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

        public static IQueryable<RecipeListItemDto> SortDtos(this IQueryable<RecipeListItemDto> recipes, string sort)
        {
            switch (sort)
            {
                case "ascending":
                    return recipes.OrderBy(recipe => recipe.Name);

                case "descending":
                    return recipes.OrderByDescending(recipe => recipe.Name);

                default:
                    return recipes.OrderBy(recipe => recipe.Id);
            }
        }
    }
}
