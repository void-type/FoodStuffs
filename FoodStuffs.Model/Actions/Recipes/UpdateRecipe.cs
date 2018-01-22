using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Model.Services.DateTime;
using Core.Model.Validation;
using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class UpdateRecipe : AbstractActionStep
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

            AddCategoriesAndCategoryRecipes(savedRecipe.Id);
            _data.SaveChanges();

            respond.WithSuccess("Recipe saved.", $"RecipeId: {_viewModel.Id}");
        }

        private readonly IFoodStuffsData _data;
        private readonly IDateTimeService _now;
        private readonly int _userId;
        private readonly RecipeViewModel _viewModel;

        private static IEnumerable<ICategory> FindUnusedCategories(IRecipe recipe, IEnumerable<ICategoryRecipe> unusedCategoryRecipes)
        {
            var categories = unusedCategoryRecipes.Select(cr => cr.Category);

            foreach (var category in categories)
            {
                if (category.CategoryRecipe.All(cr => cr.RecipeId == recipe.Id))
                {
                    yield return category;
                }
            }
        }

        private static IEnumerable<ICategoryRecipe> FindUnusedCategoryRecipes(IRecipe recipe, IRecipeViewModel viewModel)
        {
            var newCategoryNames = viewModel.Categories.Select(c => c.ToUpper().Trim()).ToList();

            var unusedCategoryRecipes =
                recipe.CategoryRecipe.Where(cr => !newCategoryNames.Contains(cr.Category.Name.ToUpper().Trim()));

            return unusedCategoryRecipes;
        }

        private void AddCategoriesAndCategoryRecipes(int recipeId)
        {
            foreach (var viewModelCategoryName in _viewModel.Categories)
            {
                var category = _data.Categories.Stored.GetByName(viewModelCategoryName) ?? CreateCategory(viewModelCategoryName);

                var existingCategoryRecipe = _data.CategoryRecipes.Stored.GetById(recipeId, category.Id);

                if (existingCategoryRecipe == null)
                {
                    CreateCategoryRecipe(recipeId, category);
                }
            }
        }

        private void CleanupCategories(IRecipe recipe)
        {
            var unusedCategoryRecipes = FindUnusedCategoryRecipes(recipe, _viewModel).ToList();
            var unusedCategories = FindUnusedCategories(recipe, unusedCategoryRecipes).ToList();

            _data.CategoryRecipes.RemoveRange(unusedCategoryRecipes);
            _data.Categories.RemoveRange(unusedCategories);
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
    }
}