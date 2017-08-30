using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Interfaces.Services.DateTime;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.ViewModels;
using System.Collections.Generic;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class CreateRecipe : ActionStep
    {
        private readonly IFoodStuffsData _data;
        private readonly IDateTimeService _now;
        private readonly IUser _user;
        private readonly RecipeViewModel _viewModel;

        public CreateRecipe(IFoodStuffsData data, IDateTimeService now, IUser user, RecipeViewModel viewModel)
        {
            _data = data;
            _now = now;
            _user = user;
            _viewModel = viewModel;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var newRecipe = _data.Recipes.New;
            TransferProperties(newRecipe);
            _data.Recipes.Add(newRecipe);

            var newCategoryRecipes = GetCategoryRecipes(newRecipe);
            _data.CategoryRecipes.AddRange(newCategoryRecipes);
        }

        private void TransferProperties(IRecipe recipe)
        {
            recipe.CreatedOn = _now.Now;
            recipe.ModifiedOn = _now.Now;
            recipe.CreatedByUserId = _user.Id;
            recipe.ModifiedByUserId = _user.Id;
            recipe.CookTimeMinutes = _viewModel.CookTimeMinutes;
            recipe.PrepTimeMinutes = _viewModel.PrepTimeMinutes;
            recipe.Directions = _viewModel.Directions;
            recipe.Ingredients = _viewModel.Ingredients;
            recipe.Name = _viewModel.Name;
        }

        private IEnumerable<ICategoryRecipe> GetCategoryRecipes(IRecipe recipe)
        {
            foreach (var viewModelCategory in _viewModel.Categories)
            {
                var category = GetOrCreateCategory(viewModelCategory);

                var categoryRecipe = _data.CategoryRecipes.New;
                categoryRecipe.RecipeId = recipe.Id;
                categoryRecipe.CategoryId = category.Id;
                yield return categoryRecipe;
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
    }
}