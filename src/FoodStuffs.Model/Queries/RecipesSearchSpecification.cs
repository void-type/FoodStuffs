using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Domain.Recipes;
using System;
using System.Linq.Expressions;
using VoidCore.Model.Queries;

namespace FoodStuffs.Model.Queries
{
    public class RecipesSearchSpecification : QuerySpecificationAbstract<Recipe>
    {
        public RecipesSearchSpecification(ListRecipes.Request request, params Expression<Func<Recipe, bool>>[] criteria) : base(criteria)
        {
            AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");

            switch (request.NameSort)
            {
                case "ascending":
                    ApplyOrderBy(recipe => recipe.Name);
                    break;

                case "descending":
                    ApplyOrderByDescending(recipe => recipe.Name);
                    break;

                default:
                    ApplyOrderBy(recipe => recipe.Id);
                    break;
            }
        }
    }
}
