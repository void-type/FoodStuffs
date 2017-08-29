using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Validation.Core;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class DeleteRecipe : ActionStep
    {
        private readonly IFoodStuffsData _data;
        private readonly int _id;

        public DeleteRecipe(IFoodStuffsData data, int id)
        {
            _data = data;
            _id = id;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var recipe = _data.Recipes.Stored.FirstOrDefault(rec => rec.Id == _id);

            if (recipe == null)
            {
                respond.WithValidationErrors($"RecipeId: {_id}", new ValidationError("Recipe does not exist."));
                return;
            }

            var recipeCategories = recipe.CategoryRecipe.ToList();
            var categories = recipeCategories.Select(recCat => recCat.Category).ToList();

            _data.CategoryRecipes.RemoveRange(recipeCategories);
            _data.Recipes.Remove(recipe);

            // Cleanup unused categories
            foreach (var category in categories)
            {
                if (category.CategoryRecipe.All(catRec => catRec.RecipeId == recipe.Id))
                {
                    _data.Categories.Remove(category);
                }
            }
        }
    }
}