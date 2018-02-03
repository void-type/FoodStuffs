using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Model.Services.DateTime;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.ViewModels;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class CreateRecipe : AbstractActionStep
    {
        public CreateRecipe(IFoodStuffsData data, IDateTimeService now, IRecipeViewModel viewModel, int userId)
        {
            _data = data;
            _now = now;
            _viewModel = viewModel;
            _userId = userId;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var newRecipe = _data.Recipes.New;
            newRecipe.CreatedOn = _now.Moment;
            newRecipe.ModifiedOn = _now.Moment;
            newRecipe.CreatedByUserId = _userId;
            newRecipe.ModifiedByUserId = _userId;
            newRecipe.CookTimeMinutes = _viewModel.CookTimeMinutes;
            newRecipe.PrepTimeMinutes = _viewModel.PrepTimeMinutes;
            newRecipe.Directions = _viewModel.Directions;
            newRecipe.Ingredients = _viewModel.Ingredients;
            newRecipe.Name = _viewModel.Name;

            _data.Recipes.Add(newRecipe);
            AddCategoriesAndCategoryRecipes(newRecipe.Id, _viewModel);

            _data.SaveChanges();

            respond.WithPostSuccess("Recipe created.", newRecipe.Id.ToString());
        }

        private readonly IFoodStuffsData _data;
        private readonly IDateTimeService _now;
        private readonly int _userId;
        private readonly IRecipeViewModel _viewModel;

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
    }
}