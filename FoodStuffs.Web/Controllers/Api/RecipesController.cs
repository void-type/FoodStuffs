using Core.Model.Actions.Chain;
using Core.Model.Actions.Steps;
using Core.Model.Time;
using Core.Services.Action;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Validation;
using FoodStuffs.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FoodStuffs.Web.Controllers.Api
{
    [Route("api/recipes")]
    public class RecipesController : Controller
    {
        public RecipesController(HttpActionResultResponder responder, IFoodStuffsData data, IDateTimeService now)
        {
            _responder = responder;
            _data = data;
            _now = now;
        }

        [HttpPut]
        public IActionResult Create([FromBody] RecipeViewModel viewModel)
        {
            new ActionChain(_responder)
                .Execute(new Validate<IRecipeViewModel>(new RecipeViewModelValidator(), viewModel))
                .Execute(new CreateRecipe(_data, _now, viewModel, 1));

            return _responder.Response;
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            new ActionChain(_responder)
                .Execute(new DeleteRecipe(_data, id));

            return _responder.Response;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            new ActionChain(_responder)
                .Execute(new RespondWithRecipeById(_data, id));

            return _responder.Response;
        }

        [Route("list")]
        [HttpGet]
        public IActionResult List(string nameSearch = null, string categorySearch = null, string sort = null, int take = int.MaxValue, int page = 1)
        {
            var recipesContext = new List<IRecipeViewModel>();
            var listContext = new List<IRecipeListItem>();
            var logExtra = $"NameSearch: {nameSearch}, CategorySearch: {categorySearch}, Sort: {sort}, Take: {take}, Page: {page}";

            new ActionChain(_responder)
                .Execute(new SearchRecipes(_data, nameSearch, categorySearch, recipesContext))
                .Execute(new SortRecipes(sort, recipesContext))
                .Execute(new ConvertRecipesToListItems(recipesContext, listContext))
                .Execute(new RespondWithPaginatedSet<IRecipeListItem>(listContext, take, page, logExtra));

            return _responder.Response;
        }

        [HttpPost]
        public IActionResult Update([FromBody] RecipeViewModel viewModel)
        {
            new ActionChain(_responder)
                .Execute(new Validate<IRecipeViewModel>(new RecipeViewModelValidator(), viewModel))
                .Execute(new UpdateRecipe(_data, _now, viewModel, 1));

            return _responder.Response;
        }

        private readonly IFoodStuffsData _data;
        private readonly IDateTimeService _now;
        private readonly HttpActionResultResponder _responder;
    }
}