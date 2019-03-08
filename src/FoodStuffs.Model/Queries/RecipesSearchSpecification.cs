using FoodStuffs.Model.Data.Models;
using System;
using System.Linq.Expressions;
using VoidCore.Model.Queries;

namespace FoodStuffs.Model.Queries
{
    public class RecipesSearchSpecification : QuerySpecificationAbstract<Recipe>
    {
        public RecipesSearchSpecification(Expression<Func<Recipe, bool>>[] criteria, string sort = null, bool pagingEnabled = false, int page = 1, int take = 1) : base(criteria)
        {
            AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");

            if (pagingEnabled)
            {
                ApplyPaging(page, take);
            }

            switch (sort)
            {
                case "name":
                    ApplyOrderBy(recipe => recipe.Name);
                    break;

                case "nameDesc":
                    ApplyOrderByDescending(recipe => recipe.Name);
                    break;

                default:
                    ApplyOrderBy(recipe => recipe.Id);
                    break;
            }
        }
    }
}
