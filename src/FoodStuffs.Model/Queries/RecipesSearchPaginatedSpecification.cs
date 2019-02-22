using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Domain.Recipes;
using System;
using System.Linq.Expressions;

namespace FoodStuffs.Model.Queries
{
    public class RecipesSearchPaginatedSpecification : RecipesSearchSpecification
    {
        public RecipesSearchPaginatedSpecification(ListRecipes.Request request, params Expression<Func<Recipe, bool>>[] criteria) : base(request, criteria)
        {
            ApplyPaging(request.Page, request.Take);
        }
    }
}
