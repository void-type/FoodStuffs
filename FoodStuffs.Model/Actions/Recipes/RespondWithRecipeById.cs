using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using System.Linq;
using VoidCore.Model.Actions.Responder;
using VoidCore.Model.Actions.Steps;

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

            respond.WithItem(savedRecipe.ToViewModel());
        }

        private readonly IFoodStuffsData _data;
        private readonly int _id;
    }
}
