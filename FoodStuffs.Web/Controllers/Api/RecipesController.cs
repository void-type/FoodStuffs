using FoodStuffs.Model.Actions.Core.Chain;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Interfaces.Services.DateTime;
using FoodStuffs.Model.Interfaces.Services.Logging;
using FoodStuffs.Model.Validation;
using FoodStuffs.Model.ViewModels;
using FoodStuffs.Web.Actions;
using Microsoft.AspNetCore.Mvc;

namespace FoodStuffs.Web.Controllers.Api
{
    [Route("api/recipes")]
    public class RecipesController : Controller
    {
        public RecipesController(ActionResultResponder responder, ILoggingService logger, IFoodStuffsData data,
            IDateTimeService now)
        {
            _responder = responder;
            _data = data;
            _now = now;
        }

        [HttpPut]
        public IActionResult Create(RecipeViewModel viewModel)
        {
            new ActionChain(_responder)
                .Execute(new Validate<RecipeViewModel>(new RecipeValidator(), viewModel))
                .Execute(new CreateRecipe(_data, _now, 1, viewModel));

            return _responder.Response;
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            new ActionChain(_responder)
                .Execute(new DeleteRecipe(_data, id));

            return _responder.Response;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            new ActionChain(_responder)
                .Execute(new RespondWithRecipeById(_data, id));

            return _responder.Response;
        }

        [Route("list")]
        [HttpGet]
        public IActionResult List()
        {
            new ActionChain(_responder)
                .Execute(new RespondWithAllRecipes(_data));

            return _responder.Response;
        }

        [HttpPost]
        public IActionResult Update([FromBody]RecipeViewModel viewModel)
        {
            new ActionChain(_responder)
                .Execute(new Validate<RecipeViewModel>(new RecipeValidator(), viewModel))
                .Execute(new UpdateRecipe(_data, _now, viewModel, 1));
            return _responder.Response;
        }

        private readonly IFoodStuffsData _data;
        private readonly IDateTimeService _now;
        private readonly ActionResultResponder _responder;
    }
}