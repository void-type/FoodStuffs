using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Interfaces.Services.Data;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class RespondWithAllRecipes : ActionStep
    {
        private readonly IFoodStuffsData _data;

        public RespondWithAllRecipes(IFoodStuffsData data)
        {
            _data = data;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.WithDataList(_data.Recipes.Stored);
        }
    }
}