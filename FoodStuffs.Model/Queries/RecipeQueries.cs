using FoodStuffs.Model.Data.Models;
using System.Collections.Generic;
using System.Linq;
using static FoodStuffs.Model.DomainEvents.Recipes.ListRecipes;

namespace FoodStuffs.Model.Queries
{
    public static class RecipeQueries
    {
        public static IQueryable<Recipe> WhereById(this IQueryable<Recipe> recipes, int id)
        {
            return recipes.Where(r => r.Id == id);
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
