using FoodStuffs.Model.Data.Models;
using System.Linq;
using VoidCore.Domain;
using static FoodStuffs.Model.Domain.Recipes.ListRecipes;

namespace FoodStuffs.Model.Queries
{
    public static class RecipeQueries
    {
        public static Maybe<Recipe> GetById(this IQueryable<Recipe> recipes, int id)
        {
            return recipes.FirstOrDefault(r => r.Id == id);
        }

        public static IQueryable<RecipeListItemDto> SortListItemDtosByName(this IQueryable<RecipeListItemDto> recipes, string sort)
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
