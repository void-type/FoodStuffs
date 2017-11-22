using FoodStuffs.Model.Interfaces.Domain;
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

        public static IEnumerable<RecipeViewModel> ToViewModel(this IQueryable<IRecipe> recipes)
        {
            // TODO: make sure all properties are here.
            // TODO: make a version for a single recipe.
            return recipes.AsEnumerable().Select(r => new RecipeViewModel
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