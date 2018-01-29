using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using FoodStuffs.Model.Data;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class RespondWithRecipeById : AbstractActionStep

    {
        public RespondWithRecipeById(IFoodStuffsData data, int id)
        {
            _data = data;
            _id = id;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var savedRecipe = _data.Recipes.Stored.FirstOrDefault(recipe => recipe.Id == _id);

            if (savedRecipe == null)
            {
                respond.WithError("Recipe not found.", $"RecipeId: {_id}");
                return;
            }

            respond.WithData(savedRecipe);
        }

        private readonly IFoodStuffsData _data;
        private readonly int _id;
    }
}