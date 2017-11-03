using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Interfaces.Services.DateTime;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.ViewModels;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class CreateRecipe : ActionStep
    {
        public CreateRecipe(IFoodStuffsData data, IDateTimeService now, int userId, RecipeViewModel viewModel)
        {
            _data = data;
            _now = now;
            _userId = userId;
            _viewModel = viewModel;
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

            AddCategoriesAndCategoryRecipes(newRecipe.Id);

            _data.SaveChanges();
        }

        private readonly IFoodStuffsData _data;
        private readonly IDateTimeService _now;
        private readonly int _userId;
        private readonly RecipeViewModel _viewModel;

        private void AddCategoriesAndCategoryRecipes(int recipeId)
        {
            foreach (var categoryName in _viewModel.Categories)
            {
                var category = _data.Categories.Stored.GetByName(categoryName);

                if (category == null)
                {
                    category = _data.Categories.New;
                    category.Name = categoryName;
                    _data.Categories.Add(category);
                }

                var categoryRecipe = _data.CategoryRecipes.New;
                categoryRecipe.RecipeId = recipeId;
                categoryRecipe.CategoryId = category.Id;
                _data.CategoryRecipes.Add(categoryRecipe);
            }
        }
    }
}