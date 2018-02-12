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
        public SearchRecipes(IFoodStuffsData data, string nameSearch, string categorySearch, string sort, List<IRecipeViewModel> recipeViewModelsContext)
        {
            _data = data;
            _nameSearch = nameSearch;
            _categorySearch = categorySearch;
            _sort = sort;
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

            if (_sort == "ascending")
            {
                list = list.OrderBy(recipe => recipe.Name);
            }
            else if (_sort == "descending")
            {
                list = list.OrderByDescending(recipe => recipe.Name);
            }

            _recipeViewModelsContext.Clear();
            _recipeViewModelsContext.AddRange(list.ToList().ToViewModel());
        }

        private readonly string _categorySearch;
        private readonly IFoodStuffsData _data;
        private readonly string _nameSearch;
        private readonly List<IRecipeViewModel> _recipeViewModelsContext;
        private readonly string _sort;
    }
}