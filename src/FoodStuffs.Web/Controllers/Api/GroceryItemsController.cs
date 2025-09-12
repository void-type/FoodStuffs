using FoodStuffs.Model.Events.GroceryItems;
using FoodStuffs.Model.Events.GroceryItems.Models;
using FoodStuffs.Model.Search;
using FoodStuffs.Model.Search.GroceryItems.Models;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage grocery items.
/// </summary>
[Route(ApiRouteAttribute.BasePath + "/grocery-items")]
public class GroceryItemsController : ControllerBase
{
    /// <summary>
    /// Search for grocery items using the following criteria. All are optional and some have defaults.
    /// </summary>
    /// <param name="searchHandler"></param>
    /// <param name="searchText">Search text (case-insensitive)</param>
    /// <param name="storageLocations">Storage location IDs to filter on</param>
    /// <param name="matchAllStorageLocations">When true, grocery items returned will match all selected storage locations</param>
    /// <param name="groceryAisles">Grocery aisle IDs to filter on</param>
    /// <param name="isOutOfStock">If the grocery items are out of stock</param>
    /// <param name="isUnused">If the grocery items have no relations</param>
    /// <param name="sortBy">Field name to sort by (case-insensitive). Options are: newest, oldest, a-z, z-a, random. Default if empty is search score.</param>
    /// <param name="randomSortSeed">Give a seed for stable random sorting. By default is stable for 24 hours on the server.</param>
    /// <param name="isPagingEnabled">Set false to get all results</param>
    /// <param name="page">The page of results to retrieve</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet]
    [ProducesResponseType(typeof(SearchGroceryItemsResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SearchAsync(
        [FromServices] SearchGroceryItemsHandler searchHandler,
        [FromQuery] string? searchText = null,
        [FromQuery] int[]? storageLocations = null,
        [FromQuery] bool matchAllStorageLocations = false,
        [FromQuery] int[]? groceryAisles = null,
        [FromQuery] bool? isOutOfStock = null,
        [FromQuery] bool? isUnused = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? randomSortSeed = null,
        [FromQuery] bool isPagingEnabled = true,
        [FromQuery] int page = 1,
        [FromQuery] int take = 30)
    {
        var request = new SearchGroceryItemsRequest(
            SearchText: searchText,
            StorageLocationIds: storageLocations,
            MatchAllStorageLocations: matchAllStorageLocations,
            GroceryAisleIds: groceryAisles,
            IsOutOfStock: isOutOfStock,
            IsUnused: isUnused,
            SortBy: sortBy,
            RandomSortSeed: randomSortSeed,
            IsPagingEnabled: isPagingEnabled,
            Page: page,
            Take: take);

        return await searchHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Suggest grocery items based on search.
    /// </summary>
    /// <param name="suggestHandler"></param>
    /// <param name="searchText">Search text (case-insensitive)</param>
    /// <param name="isPagingEnabled">Set false to get all results</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet("suggest")]
    [ProducesResponseType(typeof(IItemSet<SuggestGroceryItemsResultItem>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SuggestAsync(
        [FromServices] SuggestGroceryItemsHandler suggestHandler,
        [FromQuery] string? searchText = null,
        [FromQuery] bool isPagingEnabled = true,
        [FromQuery] int take = 8)
    {
        var request = new SuggestGroceryItemsRequest(
            SearchText: searchText,
            IsPagingEnabled: isPagingEnabled,
            Page: 1,
            Take: take);

        return await suggestHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Get a grocery item.
    /// </summary>
    /// <param name="getHandler"></param>
    /// <param name="id">The ID of the grocery item to get</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetGroceryItemResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> GetAsync([FromServices] GetGroceryItemHandler getHandler, int id)
    {
        var request = new GetGroceryItemRequest(id);

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
    public async Task<IActionResult> SaveAsync([FromServices] SaveGroceryItemHandler saveHandler, [FromBody] SaveGroceryItemRequest request)
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
    public async Task<IActionResult> SaveInventoryAsync([FromServices] SaveGroceryItemInventoryHandler saveHandler, [FromBody] SaveGroceryItemInventoryRequest request)
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
    public async Task<IActionResult> DeleteAsync([FromServices] DeleteGroceryItemHandler deleteHandler, int id)
    {
        var request = new DeleteGroceryItemRequest(id);

        return await deleteHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Rebuild the grocery item search index.
    /// </summary>
    [HttpPost("rebuild-index")]
    [ProducesResponseType(typeof(UserMessage), 200)]
    public async Task<IActionResult> RebuildAsync([FromServices] ISearchIndexService searchIndex, CancellationToken cancellationToken)
    {
        await searchIndex.RebuildAsync(SearchIndex.GroceryItems, cancellationToken);

        return HttpResponder.Respond(Result.Ok(new UserMessage("Grocery item index queued for rebuild.")));
    }
}
