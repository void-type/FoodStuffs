using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Interfaces.Services.DateTime;
using FoodStuffs.Model.ViewModels;
using System.Linq;

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
            var recipe = _data.Recipes.New;
            recipe.CreatedOn = _now.Now;
            recipe.ModifiedOn = _now.Now;
            recipe.CreatedByUserId = _user.Id;
            recipe.ModifiedByUserId = _user.Id;
            recipe.CookTimeMinutes = _viewModel.CookTimeMinutes;
            recipe.PrepTimeMinutes = _viewModel.PrepTimeMinutes;
            recipe.Directions = _viewModel.Directions;
            recipe.Ingredients = _viewModel.Ingredients;
            recipe.Name = _viewModel.Name;

            _data.Recipes.Add(recipe);

            foreach (var viewModelCategory in _viewModel.Categories)
            {
                var category = _data.Categories.Stored
                    .FirstOrDefault(x => x.Name.ToUpper().Trim() == viewModelCategory.Name.ToUpper().Trim());

                if (category == null)
                {
                    category = _data.Categories.New;
                    category.Name = viewModelCategory.Name;
                    _data.Categories.Add(category);
                }

                var categoryRecipes = _data.CategoryRecipes.New;
                categoryRecipes.RecipeId = recipe.Id;
                categoryRecipes.CategoryId = category.Id;
                _data.CategoryRecipes.Add(categoryRecipes);
            }
        }
    }
}