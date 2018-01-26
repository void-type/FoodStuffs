using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Queries;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class RespondWithRecipes : AbstractActionStep
    {
        public RespondWithRecipes(IFoodStuffsData data, string nameSearch = null, int? categoryId = null)
        {
            _data = data;
            _nameSearch = nameSearch;
            _categoryId = categoryId;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var list = _data.Recipes.Stored;

            if (!string.IsNullOrWhiteSpace(_nameSearch))
            {
                list = list.SearchNames(_nameSearch);
            }

            if (_categoryId != null)
            {
                list = list.ForCategory(_categoryId.Value);
            }

            respond.WithDataList(list.AsEnumerable().ToViewModel());
        }

        private readonly int? _categoryId;
        private readonly IFoodStuffsData _data;
        private readonly string _nameSearch;
    }
}