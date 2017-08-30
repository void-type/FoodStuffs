using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.Validation.Core;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class DeleteRecipe : ActionStep
    {
        private readonly IFoodStuffsData _data;
        private readonly int _id;

        public DeleteRecipe(IFoodStuffsData data, int id)
        {
            _data = data;
            _id = id;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var recipe = _data.Recipes.Stored.GetById(_id);

            if (recipe == null)
            {
                respond.WithValidationErrors($"RecipeId: {_id}", new ValidationError("Recipe does not exist."));
                return;
            }

            var recipeCategories = recipe.CategoryRecipe;
            _data.CategoryRecipes.RemoveRange(recipeCategories);

            var unusedCategories = FindUnusedCategories(recipe);
            _data.Categories.RemoveRange(unusedCategories);

            _data.Recipes.Remove(recipe);
        }

        private static IEnumerable<ICategory> FindUnusedCategories(IRecipe recipe)
        {
            var categories = recipe.CategoryRecipe.Select(cr => cr.Category);

            foreach (var category in categories)
            {
                if (category.CategoryRecipe.All(cr => cr.RecipeId == recipe.Id))
                {
                    yield return category;
                }
            }
        }
    }
}