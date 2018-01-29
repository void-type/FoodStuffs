using Core.Model.Actions.Responder;
using Core.Model.Actions.Responses.MessageString;
using Core.Model.Actions.Steps;
using Core.Model.Services.DateTime;
using Core.Model.Validation;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class UpdateRecipe : AbstractActionStep
    {
        public UpdateRecipe(IFoodStuffsData data, IDateTimeService now, RecipeViewModel viewModel, int userId)
        {
            _data = data;
            _now = now;
            _userId = userId;
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

            var unusedCategoryRecipes = FindUnusedCategoryRecipes(savedRecipe, _viewModel).ToList();
            var unusedCategories = FindUnusedCategories(savedRecipe, unusedCategoryRecipes).ToList();
            _data.CategoryRecipes.RemoveRange(unusedCategoryRecipes);
            _data.Categories.RemoveRange(unusedCategories);

            AddCategoriesAndCategoryRecipes(savedRecipe.Id, _viewModel);
            _data.SaveChanges();

            var response = new PostSuccessMessage
            {
                Id = _viewModel.Id.ToString(),
                Message = "Recipe saved."
            };

            respond.WithData(response, $"UserId: {_userId} RecipeId: {_viewModel.Id}");
        }

        private readonly IFoodStuffsData _data;
        private readonly IDateTimeService _now;
        private readonly int _userId;
        private readonly RecipeViewModel _viewModel;

        private void AddCategoriesAndCategoryRecipes(int recipeId, IRecipeViewModel viewModel)
        {
            foreach (var viewModelCategoryName in viewModel.Categories)
            {
                var category = _data.Categories.Stored.GetByName(viewModelCategoryName) ?? CreateCategory(viewModelCategoryName);

                var existingCategoryRecipe = _data.CategoryRecipes.Stored.GetById(recipeId, category.Id);

                if (existingCategoryRecipe == null)
                {
                    CreateCategoryRecipe(recipeId, category);
                }
            }
        }

        private ICategory CreateCategory(string viewModelCategory)
        {
            var existingCategory = _data.Categories.New;
            existingCategory.Name = viewModelCategory;
            _data.Categories.Add(existingCategory);
            return existingCategory;
        }

        private void CreateCategoryRecipe(int recipeId, ICategory existingCategory)
        {
            var categoryRecipe = _data.CategoryRecipes.New;
            categoryRecipe.RecipeId = recipeId;
            categoryRecipe.CategoryId = existingCategory.Id;
            _data.CategoryRecipes.Add(categoryRecipe);
        }

        private IEnumerable<ICategory> FindUnusedCategories(IRecipe recipe, IEnumerable<ICategoryRecipe> unusedCategoryRecipes)
        {
            var categoryIds = unusedCategoryRecipes.Select(cr => cr.CategoryId);

            var categories = _data.Categories.Stored.Where(category => categoryIds.Contains(category.Id));

            foreach (var category in categories)
            {
                if (category.CategoryRecipes.All(cr => cr.RecipeId == recipe.Id))
                {
                    yield return category;
                }
            }
        }

        private IEnumerable<ICategoryRecipe> FindUnusedCategoryRecipes(IRecipe recipe, IRecipeViewModel viewModel)
        {
            var newCategoryNames = viewModel.Categories.Select(c => c.ToUpper().Trim()).ToList();

            var unusedCategoryRecipes =
                recipe.CategoryRecipes.Where(cr => !newCategoryNames.Contains(cr.Category.Name.ToUpper().Trim()));

            return unusedCategoryRecipes;
        }
    }
}