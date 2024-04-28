using FoodStuffs.Model.Events.MealSets;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage meals.
/// </summary>
[Route(ApiRouteAttribute.BasePath + "/mealSets")]
public class MealSetsController : ControllerBase
{
    /// <summary>
    /// Search for meal sets using the following criteria. All are optional and some have defaults.
    /// </summary>
    /// <param name="listPipeline"></param>
    /// <param name="name">Name contains (case-insensitive)</param>
    /// <param name="isPagingEnabled">False for all results</param>
    /// <param name="page">The page of results to retrieve</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet]
    [ProducesResponseType(typeof(IItemSet<ListMealSetsResponse>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> Search([FromServices] ListMealSetsPipeline listPipeline, string? name = null, bool isPagingEnabled = true, int page = 1, int take = 30)
    {
        var request = new ListMealSetsRequest(
            NameSearch: name,
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
    /// Get a meal set.
    /// </summary>
    /// <param name="getPipeline"></param>
    /// <param name="id">The ID of the meal set to get</param>
    [Route("{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetMealSetResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public Task<IActionResult> Get([FromServices] GetMealSetPipeline getPipeline, int id)
    {
        var request = new GetMealSetRequest(id);

        return getPipeline
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Save a meal set. Will update if found, otherwise a new meal set will be created.
    /// </summary>
    /// <param name="savePipeline"></param>
    /// <param name="request">The meal set to save</param>
    [HttpPost]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public Task<IActionResult> Save([FromServices] SaveMealSetPipeline savePipeline, [FromBody] SaveMealSetRequest request)
    {
        return savePipeline
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Delete a meal set.
    /// </summary>
    /// <param name="deletePipeline"></param>
    /// <param name="id">The ID of the meal set</param>
    [Route("{id}")]
    [HttpDelete]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public Task<IActionResult> Delete([FromServices] DeleteMealSetPipeline deletePipeline, int id)
    {
        var request = new DeleteMealSetRequest(id);

        return deletePipeline
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }
}
