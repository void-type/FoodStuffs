using FoodStuffs.Model.Events.PantryLocations;
using FoodStuffs.Model.Events.PantryLocations.Models;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage grocery item storage locations.
/// </summary>
[Route(ApiRouteAttribute.BasePath + "/pantry-locations")]
public class PantryLocationsController : ControllerBase
{
    /// <summary>
    /// List storage locations. All parameters are optional and some have defaults.
    /// </summary>
    /// <param name="listHandler"></param>
    /// <param name="name">Name contains (case-insensitive)</param>
    /// <param name="isUnused">Specify to show items that have relations or no relations</param>
    /// <param name="isPagingEnabled">Set false to get all results</param>
    /// <param name="page">The page of results to retrieve</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet]
    [ProducesResponseType(typeof(IItemSet<ListPantryLocationsResponse>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> ListAsync([FromServices] ListPantryLocationsHandler listHandler, string? name = null, bool? isUnused = null, bool isPagingEnabled = true, int page = 1, int take = 30)
    {
        var request = new ListPantryLocationsRequest(
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
    /// Get a storage location.
    /// </summary>
    /// <param name="getHandler"></param>
    /// <param name="id">The ID of the storage location to get</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetPantryLocationResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> GetAsync([FromServices] GetPantryLocationHandler getHandler, int id)
    {
        var request = new GetPantryLocationRequest(id);

        return await getHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Save a storage location. Will update if found, otherwise a new item will be created.
    /// </summary>
    /// <param name="saveHandler"></param>
    /// <param name="request">The storage location to save</param>
    [HttpPost]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SaveAsync([FromServices] SavePantryLocationHandler saveHandler, [FromBody] SavePantryLocationRequest request)
    {
        return await saveHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Delete a storage location.
    /// </summary>
    /// <param name="deleteHandler"></param>
    /// <param name="id">The ID of the storage location</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> DeleteAsync([FromServices] DeletePantryLocationHandler deleteHandler, int id)
    {
        var request = new DeletePantryLocationRequest(id);

        return await deleteHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }
}
