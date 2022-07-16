using FoodStuffs.Model.Events.Recipes;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage recipes.
/// </summary>
[ApiRoute("recipes")]
public class RecipesController : ControllerBase
{
    /// <summary>
    /// Search for recipes using the following criteria. All are optional and some have defaults.
    /// </summary>
    /// <param name="listPipeline"></param>
    /// <param name="name">Name contains (case-insensitive)</param>
    /// <param name="category">Category names contain (case-insensitive)</param>
    /// <param name="isForMealPlanning">If the recipes should be enabled for meal planning</param>
    /// <param name="sortBy">Field name to sort by (case-insensitive). Options are: name. Default is ID</param>
    /// <param name="sortDesc">True for descending sort</param>
    /// <param name="isPagingEnabled">False for all results</param>
    /// <param name="page">The page of results to retrieve</param>
    /// <param name="take">How many items in a page</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IItemSet<ListRecipesResponse>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> Search([FromServices] ListRecipesPipeline listPipeline, string? name = null, string? category = null, bool? isForMealPlanning = null, string? sortBy = null, bool sortDesc = false, bool isPagingEnabled = true, int page = 1, int take = 30)
    {
        var request = new ListRecipesRequest(
            NameSearch: name,
            CategorySearch: category,
            SortBy: sortBy,
            IsForMealPlanning: isForMealPlanning,
            SortDesc: sortDesc,
            IsPagingEnabled: isPagingEnabled,
            Page: page,
            Take: take);

        // Cancel long-running queries
        using var cts = new CancellationTokenSource()
            .Tee(c => c.CancelAfter(5000));

        return await listPipeline
            .Handle(request, cts.Token)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Get a recipe.
    /// </summary>
    /// <param name="getPipeline"></param>
    /// <param name="id">The ID of the recipe to get</param>
    /// <returns></returns>
    [Route("{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetRecipeResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public Task<IActionResult> Get([FromServices] GetRecipePipeline getPipeline, int id)
    {
        var request = new GetRecipeRequest(id);

        return getPipeline
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Save a recipe. Will update if found, otherwise a new recipe will be created.
    /// </summary>
    /// <param name="savePipeline"></param>
    /// <param name="request">The recipe to save</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public Task<IActionResult> Save([FromServices] SaveRecipePipeline savePipeline, [FromBody] SaveRecipeRequest request)
    {
        return savePipeline
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Delete a recipe.
    /// </summary>
    /// <param name="deletePipeline"></param>
    /// <param name="id">The ID of the recipe</param>
    /// <returns></returns>
    [Route("{id}")]
    [HttpDelete]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public Task<IActionResult> Delete([FromServices] DeleteRecipePipeline deletePipeline, int id)
    {
        var request = new DeleteRecipeRequest(id);

        return deletePipeline
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }
}
