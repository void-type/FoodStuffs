using Microsoft.AspNetCore.Mvc;
using FoodStuffs.Model.DomainEvents.Recipes;
using VoidCore.AspNet.ClientApp;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("recipes")]
    public class RecipesController : Controller
    {
        public RecipesController(HttpResponder responder, GetRecipe.Handler getHandler, GetRecipe.Logging getLogging,
            ListRecipes.Handler listHandler, ListRecipes.Logging listLogging, DeleteRecipe.Handler deleteHandler, DeleteRecipe.Logging deleteLogging,
            SaveRecipe.Handler updateHandler, SaveRecipe.RequestValidator updateValidator, SaveRecipe.Logging updateLogging)
        {
            _responder = responder;
            _getHandler = getHandler;
            _getLogging = getLogging;
            _listHandler = listHandler;
            _listLogging = listLogging;
            _saveHandler = updateHandler;
            _saveValidator = updateValidator;
            _saveLogging = updateLogging;
            _deleteHandler = deleteHandler;
            _deleteLogging = deleteLogging;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            var request = new GetRecipe.Request(id);

            var result = _getHandler
                .AddPostProcessor(_getLogging)
                .Handle(request);

            return _responder.Respond(result);
        }

        [Route("list")]
        [HttpGet]
        public IActionResult List(int take = int.MaxValue, int page = 1, string nameSearch = null, string categorySearch = null, string sort = null)
        {
            var request = new ListRecipes.Request(page, take, nameSearch, categorySearch, sort);

            var result = _listHandler
                .AddPostProcessor(_listLogging)
                .Handle(request);

            return _responder.Respond(result);
        }

        [HttpPost]
        public IActionResult Save([FromBody] SaveRecipe.Request request)
        {
            var result = _saveHandler
                .AddRequestValidator(_saveValidator)
                .AddPostProcessor(_saveLogging)
                .Handle(request);

            return _responder.Respond(result);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var request = new DeleteRecipe.Request(id);

            var result = _deleteHandler
                .AddPostProcessor(_deleteLogging)
                .Handle(request);

            return _responder.Respond(result);
        }

        private readonly HttpResponder _responder;
        private readonly GetRecipe.Handler _getHandler;
        private readonly GetRecipe.Logging _getLogging;
        private readonly ListRecipes.Handler _listHandler;
        private readonly ListRecipes.Logging _listLogging;
        private readonly SaveRecipe.Handler _saveHandler;
        private readonly SaveRecipe.RequestValidator _saveValidator;
        private readonly SaveRecipe.Logging _saveLogging;
        private readonly DeleteRecipe.Handler _deleteHandler;
        private readonly DeleteRecipe.Logging _deleteLogging;
    }
}
