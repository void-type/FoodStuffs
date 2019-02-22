using FoodStuffs.Model.Data.Models;
using System;
using System.Linq.Expressions;
using VoidCore.Model.Queries;

namespace FoodStuffs.Model.Queries
{
    public class RecipesByIdWithCategoriesSpecification : QuerySpecificationAbstract<Recipe>
    {
        public RecipesByIdWithCategoriesSpecification(int id) : base(new Expression<Func<Recipe, bool>>[] { r => r.Id == id })
        {
            AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");
        }
    }
}
