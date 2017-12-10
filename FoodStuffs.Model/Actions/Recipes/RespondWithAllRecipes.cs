using System.Linq;
using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
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
            // TODO: paging or client lazy loading.
            respond.WithDataList(_data.Recipes.Stored.AsEnumerable().ToViewModel());
        }

        private readonly IFoodStuffsData _data;
    }
}