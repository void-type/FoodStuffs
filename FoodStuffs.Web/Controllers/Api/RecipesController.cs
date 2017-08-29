using FoodStuffs.Model.Actions.Core.Chain;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Interfaces.Services.Logging;
using FoodStuffs.Web.Actions;
using Microsoft.AspNetCore.Mvc;

namespace FoodStuffs.Web.Controllers.Api
{
    [Route("api/recipes")]
    public class RecipesController : Controller
    {
        private readonly ActionResultResponder _responder;
        private readonly IFoodStuffsData _data;

        public RecipesController(ActionResultResponder responder, ILoggingService logger, IFoodStuffsData data)
        {
            _responder = responder;
            _data = data;
        }

        [HttpGet]
        public IActionResult List()
        {
            new ActionChain(_responder)
                .Execute(new RespondWithAllRecipes(_data));

            return _responder.Response;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            new ActionChain(_responder)
                .Execute(new RespondWithRecipeById(_data, id));

            return _responder.Response;
        }
    }
}