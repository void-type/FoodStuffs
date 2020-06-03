using FoodStuffs.Model.Data.Models;
using System;
using System.Linq.Expressions;
using VoidCore.Model.Data;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Queries
{
    public class RecipesSearchSpecification : QuerySpecificationAbstract<Recipe>
    {
        public RecipesSearchSpecification(Expression<Func<Recipe, bool>>[] criteria) : base(criteria)
        {
            AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");
        }

        public RecipesSearchSpecification(Expression<Func<Recipe, bool>>[] criteria, PaginationOptions paginationOptions, string sort = null) : this(criteria)
        {
            ApplyPaging(paginationOptions);

            switch (sort)
            {
                case "name":
                    ApplyOrderBy(recipe => recipe.Name);
                    AddThenBy(recipe => recipe.Id);
                    break;

                case "nameDesc":
                    ApplyOrderByDescending(recipe => recipe.Name);
                    AddThenByDescending(recipe => recipe.Id);
                    break;

                default:
                    ApplyOrderBy(recipe => recipe.Id);
                    break;
            }
        }
    }
}
