using FoodStuffs.Model.Events.Recipes;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Domain;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("recipes")]
    public class RecipesController : ControllerBase
    {
        private readonly GetRecipe.Handler _getHandler;
        private readonly GetRecipe.Logger _getLogger;
        private readonly ListRecipes.Handler _listHandler;
        private readonly ListRecipes.Logger _listLogger;
        private readonly SaveRecipe.Handler _saveHandler;
        private readonly SaveRecipe.RequestValidator _saveValidator;
        private readonly SaveRecipe.Logger _saveLogger;
        private readonly DeleteRecipe.Handler _deleteHandler;
        private readonly DeleteRecipe.Logger _deleteLogger;

        public RecipesController(GetRecipe.Handler getHandler, GetRecipe.Logger getLogger,
            ListRecipes.Handler listHandler, ListRecipes.Logger listLogger, DeleteRecipe.Handler deleteHandler, DeleteRecipe.Logger deleteLogger,
            SaveRecipe.Handler updateHandler, SaveRecipe.RequestValidator updateValidator, SaveRecipe.Logger updateLogger)
        {
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

        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> List(string name = null, string category = null, string sort = null, bool isPagingEnabled = true, int page = 1, int take = 30)
        {
            var request = new ListRecipes.Request(
                nameSearch: name,
                categorySearch: category,
                sort: sort,
                isPagingEnabled: isPagingEnabled,
                page: page,
                take: take);

            // Cancel long-running queries
            using var cts = new CancellationTokenSource()
                .Tee(c => c.CancelAfter(5000));

            var result = await _listHandler
                .AddPostProcessor(_listLogger)
                .Handle(request, cts.Token)
                .MapAsync(HttpResponder.Respond);

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var request = new GetRecipe.Request(id);

            return await _getHandler
                .AddPostProcessor(_getLogger)
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SaveRecipe.Request request)
        {
            return await _saveHandler
                .AddRequestValidator(_saveValidator)
                .AddPostProcessor(_saveLogger)
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteRecipe.Request(id);

            return await _deleteHandler
                .AddPostProcessor(_deleteLogger)
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }
    }
}
