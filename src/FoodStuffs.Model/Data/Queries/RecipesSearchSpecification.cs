﻿using FoodStuffs.Model.Data.Models;
using System;
using System.Linq.Expressions;
using VoidCore.Model.Data;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Data.Queries
{
    public class RecipesSearchSpecification : QuerySpecificationAbstract<Recipe>
    {
        public RecipesSearchSpecification(Expression<Func<Recipe, bool>>[] criteria) : base(criteria)
        {
            AddInclude($"{nameof(Recipe.CategoryRecipes)}.{nameof(CategoryRecipe.Category)}");
            AddInclude(nameof(Recipe.Images));
        }

        public RecipesSearchSpecification(Expression<Func<Recipe, bool>>[] criteria, PaginationOptions paginationOptions, string? sortBy = null, bool sortDesc = false) : this(criteria)
        {
            ApplyPaging(paginationOptions);

            switch (sortBy?.ToUpperInvariant())
            {
                case "NAME":
                    AddOrderBy(recipe => recipe.Name, sortDesc);
                    AddOrderBy(recipe => recipe.Id);
                    break;

                default:
                    AddOrderBy(recipe => recipe.Id, true);
                    break;
            }
        }
    }
}
