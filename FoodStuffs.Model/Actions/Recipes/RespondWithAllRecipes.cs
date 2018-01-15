using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Queries;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class RespondWithAllRecipes : AbstractActionStep
    {
        public RespondWithAllRecipes(IFoodStuffsData data)
        {
            _data = data;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.WithDataList(_data.Recipes.Stored.AsEnumerable().ToViewModel());
        }

        private readonly IFoodStuffsData _data;
    }
}