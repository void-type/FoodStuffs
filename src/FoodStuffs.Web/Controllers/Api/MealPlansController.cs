using FoodStuffs.Model.Events.MealPlans;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage meal plans.
/// </summary>
[Route(ApiRouteAttribute.BasePath + "/meal-plans")]
public class MealPlansController : ControllerBase
{
    /// <summary>
    /// List meal plans. All parameters are optional and some have defaults.
    /// </summary>
    /// <param name="listHandler"></param>
    /// <param name="isPagingEnabled">False for all results</param>
    /// <param name="page">The page of results to retrieve</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet]
    [ProducesResponseType(typeof(IItemSet<ListMealPlansResponse>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> ListAsync([FromServices] ListMealPlansHandler listHandler, bool isPagingEnabled = true, int page = 1, int take = 30)
    {
        var request = new ListMealPlansRequest(
            IsPagingEnabled: isPagingEnabled,
            Page: page,
            Take: take);

        // Cancel long-running queries
        using var cts = new CancellationTokenSource()
            .Tee(c => c.CancelAfter(5000));

        return await listHandler
            .Handle(request, cts.Token)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Get a meal plan.
    /// </summary>
    /// <param name="getHandler"></param>
    /// <param name="id">The ID of the meal plan to get</param>
    [Route("{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetMealPlanResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> GetAsync([FromServices] GetMealPlanHandler getHandler, int id)
    {
        var request = new GetMealPlanRequest(id);

        return await getHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Save a meal plan. Will update if found, otherwise a new meal plan will be created.
    /// </summary>
    /// <param name="saveHandler"></param>
    /// <param name="request">The meal plan to save</param>
    [HttpPost]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SaveAsync([FromServices] SaveMealPlanHandler saveHandler, [FromBody] SaveMealPlanRequest request)
    {
        return await saveHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Delete a meal plan.
    /// </summary>
    /// <param name="deleteHandler"></param>
    /// <param name="id">The ID of the meal plan</param>
    [Route("{id}")]
    [HttpDelete]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> DeleteAsync([FromServices] DeleteMealPlanHandler deleteHandler, int id)
    {
        var request = new DeleteMealPlanRequest(id);

        return await deleteHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }
}
