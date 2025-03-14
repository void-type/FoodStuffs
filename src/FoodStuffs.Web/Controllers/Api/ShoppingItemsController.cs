using FoodStuffs.Model.Events.ShoppingItems;
using FoodStuffs.Model.Events.ShoppingItems.Models;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage recipe grocery items.
/// </summary>
[Route(ApiRouteAttribute.BasePath + "/shopping-items")]
public class ShoppingItemsController : ControllerBase
{
    /// <summary>
    /// List grocery items. All parameters are optional and some have defaults.
    /// </summary>
    /// <param name="listHandler"></param>
    /// <param name="name">Name contains (case-insensitive)</param>
    /// <param name="isUnused">Specify to show items that have relations or no relations</param>
    /// <param name="isPagingEnabled">Set false to get all results</param>
    /// <param name="page">The page of results to retrieve</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet]
    [ProducesResponseType(typeof(IItemSet<ListShoppingItemsResponse>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> ListAsync([FromServices] ListShoppingItemsHandler listHandler, string? name = null, bool? isUnused = null, bool isPagingEnabled = true, int page = 1, int take = 30)
    {
        var request = new ListShoppingItemsRequest(
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
    /// Get a grocery item.
    /// </summary>
    /// <param name="getHandler"></param>
    /// <param name="id">The ID of the grocery item to get</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetShoppingItemResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> GetAsync([FromServices] GetShoppingItemHandler getHandler, int id)
    {
        var request = new GetShoppingItemRequest(id);

        return await getHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Save a grocery item. Will update if found, otherwise a new item will be created.
    /// </summary>
    /// <param name="saveHandler"></param>
    /// <param name="request">The grocery item to save</param>
    [HttpPost]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SaveAsync([FromServices] SaveShoppingItemHandler saveHandler, [FromBody] SaveShoppingItemRequest request)
    {
        return await saveHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Update a grocery item inventory.
    /// </summary>
    /// <param name="saveHandler"></param>
    /// <param name="request">The grocery item to save</param>
    [HttpPost("inventory")]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SaveInventoryAsync([FromServices] SaveShoppingItemInventoryHandler saveHandler, [FromBody] SaveShoppingItemInventoryRequest request)
    {
        return await saveHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Delete a grocery item.
    /// </summary>
    /// <param name="deleteHandler"></param>
    /// <param name="id">The ID of the grocery item</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> DeleteAsync([FromServices] DeleteShoppingItemHandler deleteHandler, int id)
    {
        var request = new DeleteShoppingItemRequest(id);

        return await deleteHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }
}
