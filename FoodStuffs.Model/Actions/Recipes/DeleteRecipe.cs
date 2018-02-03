using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Model.Validation;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class DeleteRecipe : AbstractActionStep
    {
        public DeleteRecipe(IFoodStuffsData data, int recipeId)
        {
            _data = data;
            _recipeId = recipeId;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var recipe = _data.Recipes.Stored.GetById(_recipeId);

            if (recipe == null)
            {
                respond.WithValidationErrors($"RecipeId: {_recipeId}", new ValidationError("Recipe does not exist."));
                return;
            }

            var recipeCategories = recipe.CategoryRecipes;
            _data.CategoryRecipes.RemoveRange(recipeCategories);

            var unusedCategories = FindUnusedCategories(recipe);
            _data.Categories.RemoveRange(unusedCategories);

            _data.Recipes.Remove(recipe);

            _data.SaveChanges();

            respond.WithPostSuccess("Recipe deleted.", _recipeId.ToString());
        }

        private readonly IFoodStuffsData _data;
        private readonly int _recipeId;

        private IEnumerable<ICategory> FindUnusedCategories(IRecipe recipe)
        {
            var categoryIdsToCheck = recipe.CategoryRecipes.Select(cr => cr.CategoryId);

            var categoriesToCheck = _data.Categories.Stored.Where(c => categoryIdsToCheck.Contains(c.Id));

            foreach (var categoryToCheck in categoriesToCheck)
            {
                if (_data.CategoryRecipes.Stored.Where(cr => cr.CategoryId == categoryToCheck.Id)
                    .All(cr => cr.RecipeId == recipe.Id))
                {
                    yield return categoryToCheck;
                }
            }
        }
    }
}