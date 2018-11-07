using FoodStuffs.Model.Domain.Recipes;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.Attributes;
using VoidCore.AspNet.ClientApp;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("recipes")]
    public class RecipesController : Controller
    {
        public RecipesController(HttpResponder responder, GetRecipe.Handler getHandler, GetRecipe.Logger getLogger,
            ListRecipes.Handler listHandler, ListRecipes.Logger listLogger, DeleteRecipe.Handler deleteHandler, DeleteRecipe.Logger deleteLogger,
            SaveRecipe.Handler updateHandler, SaveRecipe.RequestValidator updateValidator, SaveRecipe.Logger updateLogger)
        {
            _responder = responder;
            _getHandler = getHandler;
            _getLogger = getLogger;
            _listHandler = listHandler;
            _listLogger = listLogger;
            _saveHandler = updateHandler;
            _saveValidator = updateValidator;
            _saveLogger = updateLogger;
            _deleteHandler = deleteHandler;
            _deleteLogger = deleteLogger;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            var request = new GetRecipe.Request(id);

            var result = _getHandler
                .AddPostProcessor(_getLogger)
                .Handle(request);

            return _responder.Respond(result);
        }

        [Route("list")]
        [HttpGet]
        public IActionResult List(int take = int.MaxValue, int page = 1, string nameSearch = null, string categorySearch = null, string sort = null)
        {
            var request = new ListRecipes.Request(page, take, nameSearch, categorySearch, sort);

            var result = _listHandler
                .AddPostProcessor(_listLogger)
                .Handle(request);

            return _responder.Respond(result);
        }

        [HttpPost]
        public IActionResult Save([FromBody] SaveRecipe.Request request)
        {
            var result = _saveHandler
                .AddRequestValidator(_saveValidator)
                .AddPostProcessor(_saveLogger)
                .Handle(request);

            return _responder.Respond(result);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var request = new DeleteRecipe.Request(id);

            var result = _deleteHandler
                .AddPostProcessor(_deleteLogger)
                .Handle(request);

            return _responder.Respond(result);
        }

        private readonly HttpResponder _responder;
        private readonly GetRecipe.Handler _getHandler;
        private readonly GetRecipe.Logger _getLogger;
        private readonly ListRecipes.Handler _listHandler;
        private readonly ListRecipes.Logger _listLogger;
        private readonly SaveRecipe.Handler _saveHandler;
        private readonly SaveRecipe.RequestValidator _saveValidator;
        private readonly SaveRecipe.Logger _saveLogger;
        private readonly DeleteRecipe.Handler _deleteHandler;
        private readonly DeleteRecipe.Logger _deleteLogger;
    }
}
