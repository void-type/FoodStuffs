using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Queries;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class RespondWithAllRecipes : ActionStep
    {
        public RespondWithAllRecipes(IFoodStuffsData data)
        {
            _data = data;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            // make a viewModel to prevent circular reference exceptions
            respond.WithDataList(_data.Recipes.Stored.ToViewModel());
        }

        private readonly IFoodStuffsData _data;
    }
}