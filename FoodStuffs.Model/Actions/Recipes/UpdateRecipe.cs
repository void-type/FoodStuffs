using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Interfaces.Services.DateTime;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.Validation.Core;
using FoodStuffs.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class UpdateRecipe : ActionStep
    {
        public UpdateRecipe(IFoodStuffsData data, IDateTimeService now, RecipeViewModel viewModel, int userIdId)
        {
            _data = data;
            _now = now;
            _userId = userIdId;
            _viewModel = viewModel;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var savedRecipe = _data.Recipes.Stored.GetById(_viewModel.Id);

            if (savedRecipe == null)
            {
                respond.WithValidationErrors($"RecipeId: {_viewModel.Id}",
                    new ValidationError("Recipe does not exist."));
                return;
            }

            savedRecipe.ModifiedOn = _now.Moment;
            savedRecipe.ModifiedByUserId = _userId;
            savedRecipe.CookTimeMinutes = _viewModel.CookTimeMinutes;
            savedRecipe.PrepTimeMinutes = _viewModel.PrepTimeMinutes;
            savedRecipe.Directions = _viewModel.Directions;
            savedRecipe.Ingredients = _viewModel.Ingredients;
            savedRecipe.Name = _viewModel.Name;

            CleanupCategories(savedRecipe);
            _data.SaveChanges();

            AddCategories(savedRecipe);
            _data.SaveChanges();
        }

        private readonly IFoodStuffsData _data;
        private readonly IDateTimeService _now;
        private readonly int _userId;
        private readonly RecipeViewModel _viewModel;

        private static IEnumerable<ICategory> FindUnusedCategories(IRecipe recipe,
            IEnumerable<ICategoryRecipe> categoryRecipesToBeRemoved)
        {
            var categories = categoryRecipesToBeRemoved.Select(cr => cr.Category);

            foreach (var category in categories)
            {
                if (category.CategoryRecipe.All(cr => cr.RecipeId == recipe.Id))
                {
                    yield return category;
                }
            }
        }

        private void AddCategories(IRecipe recipe)
        {
            foreach (var viewModelCategory in _viewModel.Categories)
            {
                var existingCategory = _data.Categories.Stored.GetByName(viewModelCategory.Name);

                if (existingCategory == null)
                {
                    existingCategory = _data.Categories.New;
                    existingCategory.Name = viewModelCategory.Name;

                    _data.Categories.Add(existingCategory);
                }

                var existingCategoryRecipe = _data.CategoryRecipes.Stored.GetById(recipe.Id, existingCategory.Id);

                if (existingCategoryRecipe == null)
                {
                    var categoryRecipe = _data.CategoryRecipes.New;
                    categoryRecipe.RecipeId = recipe.Id;
                    categoryRecipe.CategoryId = existingCategory.Id;

                    _data.CategoryRecipes.Add(categoryRecipe);
                }
            }
        }

        private void CleanupCategories(IRecipe recipe)
        {
            var categoryRecipesToRemove = FindUnusedCategoryRecipes(recipe);
            var unusedCategoriesToRemove = FindUnusedCategories(recipe, categoryRecipesToRemove).ToList();

            _data.CategoryRecipes.RemoveRange(categoryRecipesToRemove);
            _data.Categories.RemoveRange(unusedCategoriesToRemove);
        }

        private List<ICategoryRecipe> FindUnusedCategoryRecipes(IRecipe recipe)
        {
            return recipe.CategoryRecipe
                .Where(cr => !_viewModel.Categories
                    .Select(c => c.Name.ToUpper().Trim())
                    .Contains(cr.Category.Name.ToUpper().Trim()))
                .ToList();
        }
    }
}