using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Interfaces.Services.Data;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class RespondWithRecipeById : ActionStep

    {
        private readonly IFoodStuffsData _data;
        private readonly int _id;

        public RespondWithRecipeById(IFoodStuffsData data, int id)
        {
            _data = data;
            _id = id;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.WithData(_data.Recipes.Stored.FirstOrDefault(recipe => recipe.Id == _id));
        }
    }
}