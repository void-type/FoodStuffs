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
        private readonly IFoodStuffsData _data;
        private readonly IDateTimeService _now;
        private readonly IUser _user;
        private readonly RecipeViewModel _viewModel;

        public UpdateRecipe(IFoodStuffsData data, IDateTimeService now, IUser user, RecipeViewModel viewModel)
        {
            _data = data;
            _now = now;
            _user = user;
            _viewModel = viewModel;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var savedRecipe = _data.Recipes.Stored.GetById(_viewModel.Id);

            if (savedRecipe == null)
            {
                respond.WithValidationErrors($"RecipeId: {_viewModel.Id}", new ValidationError("Recipe does not exist."));
                return;
            }

            savedRecipe.ModifiedOn = _now.Now;
            savedRecipe.ModifiedByUserId = _user.Id;
            savedRecipe.CookTimeMinutes = _viewModel.CookTimeMinutes;
            savedRecipe.PrepTimeMinutes = _viewModel.PrepTimeMinutes;
            savedRecipe.Directions = _viewModel.Directions;
            savedRecipe.Ingredients = _viewModel.Ingredients;
            savedRecipe.Name = _viewModel.Name;

            UpdateCategories(savedRecipe);
        }

        private void UpdateCategories(IRecipe recipe)
        {
            // Remove all extra CategoryRecipes and unused Categories from recipe
            var categoryRecipesToRemove = recipe.CategoryRecipe
                .Where(cr => !_viewModel.Categories
                    .Select(c => c.Name.ToUpper().Trim())
                    .Contains(cr.Category.Name.ToUpper().Trim()))
                .ToList();
            _data.CategoryRecipes.RemoveRange(categoryRecipesToRemove);

            var unusedCategoriesToRemove = FindUnusedCategories(recipe, categoryRecipesToRemove).ToList();
            _data.Categories.RemoveRange(unusedCategoriesToRemove);

            // Add all missing Categories and CategoryRecipes from view model
            foreach (var viewModelCategory in _viewModel.Categories)
            {
                var category = GetOrCreateCategory(viewModelCategory);

                var existingCategoryRecipe = _data.CategoryRecipes.Stored.GetById(recipe.Id, category.Id);

                if (existingCategoryRecipe == null)
                {
                    var categoryRecipe = _data.CategoryRecipes.New;
                    categoryRecipe.RecipeId = recipe.Id;
                    categoryRecipe.CategoryId = category.Id;

                    _data.CategoryRecipes.Add(categoryRecipe);
                }
            }
        }

        private ICategory GetOrCreateCategory(ICategory viewModelCategory)
        {
            var category = _data.Categories.Stored
                .GetByName(viewModelCategory.Name);

            if (category == null)
            {
                category = _data.Categories.New;
                category.Name = viewModelCategory.Name;
                _data.Categories.Add(category);
            }

            return category;
        }

        private static IEnumerable<ICategory> FindUnusedCategories(IRecipe recipe, IEnumerable<ICategoryRecipe> categoryRecipesToBeRemoved)
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
    }
}