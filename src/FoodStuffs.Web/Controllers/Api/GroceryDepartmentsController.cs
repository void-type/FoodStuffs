using FoodStuffs.Model.Events.GroceryDepartments;
using FoodStuffs.Model.Events.GroceryDepartments.Models;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage grocery item grocery aisles.
/// </summary>
[Route(ApiRouteAttribute.BasePath + "/grocery-departments")]
public class GroceryDepartmentsController : ControllerBase
{
    /// <summary>
    /// List grocery aisles. All parameters are optional and some have defaults.
    /// </summary>
    /// <param name="listHandler"></param>
    /// <param name="name">Name contains (case-insensitive)</param>
    /// <param name="isUnused">Specify to show items that have relations or no relations</param>
    /// <param name="isPagingEnabled">Set false to get all results</param>
    /// <param name="page">The page of results to retrieve</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet]
    [ProducesResponseType(typeof(IItemSet<ListGroceryDepartmentsResponse>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> ListAsync([FromServices] ListGroceryDepartmentsHandler listHandler, string? name = null, bool? isUnused = null, bool isPagingEnabled = true, int page = 1, int take = 30)
    {
        var request = new ListGroceryDepartmentsRequest(
            NameSearch: name,
            IsUnused: isUnused,
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
    /// Get a grocery aisle.
    /// </summary>
    /// <param name="getHandler"></param>
    /// <param name="id">The ID of the grocery aisle to get</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetGroceryDepartmentResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> GetAsync([FromServices] GetGroceryDepartmentHandler getHandler, int id)
    {
        var request = new GetGroceryDepartmentRequest(id);

        return await getHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Save a grocery aisle. Will update if found, otherwise a new item will be created.
    /// </summary>
    /// <param name="saveHandler"></param>
    /// <param name="request">The grocery aisle to save</param>
    [HttpPost]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SaveAsync([FromServices] SaveGroceryDepartmentHandler saveHandler, [FromBody] SaveGroceryDepartmentRequest request)
    {
        return await saveHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Delete a grocery aisle.
    /// </summary>
    /// <param name="deleteHandler"></param>
    /// <param name="id">The ID of the grocery aisle</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> DeleteAsync([FromServices] DeleteGroceryDepartmentHandler deleteHandler, int id)
    {
        var request = new DeleteGroceryDepartmentRequest(id);

        return await deleteHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }
}
