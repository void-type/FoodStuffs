using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Model.Validation;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Interfaces.Services.Data.Models;
using FoodStuffs.Model.Queries;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class DeleteRecipe : AbstractActionStep
    {
        public DeleteRecipe(IFoodStuffsData data, int recipeId, int userId)
        {
            _data = data;
            _recipeId = recipeId;
            _userId = userId;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var recipe = _data.Recipes.Stored.GetById(_recipeId);

            if (recipe == null)
            {
                respond.WithValidationErrors($"RecipeId: {_recipeId}", new ValidationError("Recipe does not exist."));
                return;
            }

            var recipeCategories = recipe.CategoryRecipe;
            _data.CategoryRecipes.RemoveRange(recipeCategories);

            var unusedCategories = FindUnusedCategories(recipe);
            _data.Categories.RemoveRange(unusedCategories);

            _data.Recipes.Remove(recipe);

            _data.SaveChanges();

            respond.WithSuccess("Recipe deleted.", $"UserId: {_userId} RecipeId: {_recipeId}");
        }

        private readonly IFoodStuffsData _data;
        private readonly int _recipeId;
        private readonly int _userId;

        private IEnumerable<ICategory> FindUnusedCategories(IRecipe recipe)
        {
            var categoryIdsToCheck = recipe.CategoryRecipe.Select(cr => cr.CategoryId);

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