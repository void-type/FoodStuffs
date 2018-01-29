using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using FoodStuffs.Model.Interfaces.Data;
using FoodStuffs.Model.Queries;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class RespondWithRecipes : AbstractActionStep
    {
        public RespondWithRecipes(IFoodStuffsData data, string nameSearch = null, string categorySearch = null, int take = int.MaxValue, int page = 1)
        {
            _data = data;
            _nameSearch = nameSearch;
            _categorySearch = categorySearch;
            _take = take;
            _page = page;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var list = _data.Recipes.Stored;

            if (!string.IsNullOrWhiteSpace(_nameSearch))
            {
                list = list.SearchNames(_nameSearch);
            }

            if (!string.IsNullOrWhiteSpace(_categorySearch))
            {
                list = list.SearchCategory(_categorySearch);
            }

            list = list.Skip((_page - 1) * _take).Take(_take);

            respond.WithDataList(list.AsEnumerable().ToViewModel());
        }

        private readonly string _categorySearch;
        private readonly IFoodStuffsData _data;
        private readonly string _nameSearch;
        private readonly int _page;
        private readonly int _take;
    }
}