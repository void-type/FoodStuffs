using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class SearchRecipes : AbstractActionStep
    {
        public SearchRecipes(IFoodStuffsData data, string nameSearch, string categorySearch, List<IRecipeViewModel> recipeViewModelsContext)
        {
            _data = data;
            _nameSearch = nameSearch;
            _categorySearch = categorySearch;
            _recipeViewModelsContext = recipeViewModelsContext;
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

            _recipeViewModelsContext.Clear();
            _recipeViewModelsContext.AddRange(list.ToList().ToViewModels());
        }

        private readonly string _categorySearch;
        private readonly IFoodStuffsData _data;
        private readonly string _nameSearch;
        private readonly List<IRecipeViewModel> _recipeViewModelsContext;
    }
}