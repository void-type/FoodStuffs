using FoodStuffs.Model.Events.Recipes;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("recipes")]
    public class RecipesController : ControllerBase
    {
        private readonly GetRecipe.Pipeline _getPipeline;
        private readonly ListRecipes.Pipeline _listPipeline;
        private readonly DeleteRecipe.Pipeline _deletePipeline;
        private readonly SaveRecipe.Pipeline _savePipeline;

        public RecipesController(GetRecipe.Pipeline getPipeline, ListRecipes.Pipeline listPipeline,
            DeleteRecipe.Pipeline deletePipeline, SaveRecipe.Pipeline savePipeline)
        {
            _getPipeline = getPipeline;
            _listPipeline = listPipeline;
            _deletePipeline = deletePipeline;
            _savePipeline = savePipeline;
        }

        [Route("list")]
        [HttpGet]
        public Task<IActionResult> List(string? name = null, string? category = null, string? sortBy = null, bool sortDesc = false, bool isPagingEnabled = true, int page = 1, int take = 30)
        {
            var request = new ListRecipes.Request(
                NameSearch: name,
                CategorySearch: category,
                SortBy: sortBy,
                SortDesc: sortDesc,
                IsPagingEnabled: isPagingEnabled,
                Page: page,
                Take: take);

            // Cancel long-running queries
            using var cts = new CancellationTokenSource()
                .Tee(c => c.CancelAfter(5000));

            return _listPipeline
                .Handle(request, cts.Token)
                .MapAsync(HttpResponder.Respond);
        }

        [HttpGet]
        public Task<IActionResult> Get(int id)
        {
            var request = new GetRecipe.Request(id);

            return _getPipeline
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }

        [HttpPost]
        public Task<IActionResult> Save([FromBody] SaveRecipe.Request request)
        {
            return _savePipeline
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }

        [HttpDelete]
        public Task<IActionResult> Delete(int id)
        {
            var request = new DeleteRecipe.Request(id);

            return _deletePipeline
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }
    }
}
