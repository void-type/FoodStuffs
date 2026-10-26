using FoodStuffs.Model.Events;
using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Events.Recipes.Models;
using FoodStuffs.Model.Search;
using FoodStuffs.Model.Search.Recipes.Models;
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
[Route(ApiRouteAttribute.BasePath + "/recipes")]
public class RecipesController : ControllerBase
{
    /// <summary>
    /// Search for recipes using the following criteria. All are optional and some have defaults.
    /// </summary>
    /// <param name="searchHandler"></param>
    /// <param name="searchText">Search text (case-insensitive)</param>
    /// <param name="categories">Category IDs to filter on</param>
    /// <param name="matchAllCategories">When true, recipes returned will match all selected categories</param>
    /// <param name="groceryItemIds">Grocery item IDs to filter on</param>
    /// <param name="matchAllGroceryItems">When true, recipes returned will match all selected grocery items</param>
    /// <param name="isForMealPlanning">If the recipes should be enabled for meal planning</param>
    /// <param name="sortBy">Field name to sort by (case-insensitive). Options are: newest, oldest, a-z, z-a, random. Default if empty is search score.</param>
    /// <param name="randomSortSeed">Give a seed for stable random sorting. By default is stable for 24 hours on the server.</param>
    /// <param name="isPagingEnabled">Set false to get all results</param>
    /// <param name="page">The page of results to retrieve</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet]
    [ProducesResponseType(typeof(SearchRecipesResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SearchAsync(
        [FromServices] SearchRecipesHandler searchHandler,
        [FromQuery] string? searchText = null,
        [FromQuery] int[]? categories = null,
        [FromQuery] bool matchAllCategories = false,
        [FromQuery] int[]? groceryItemIds = null,
        [FromQuery] bool matchAllGroceryItems = false,
        [FromQuery] bool? isForMealPlanning = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? randomSortSeed = null,
        [FromQuery] bool isPagingEnabled = true,
        [FromQuery] int page = 1,
        [FromQuery] int take = 30)
    {
        var request = new SearchRecipesRequest(
            SearchText: searchText,
            CategoryIds: categories,
            MatchAllCategories: matchAllCategories,
            GroceryItemIds: groceryItemIds,
            MatchAllGroceryItems: matchAllGroceryItems,
            IsForMealPlanning: isForMealPlanning,
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
    /// Suggest recipes based on search.
    /// </summary>
    /// <param name="suggestHandler"></param>
    /// <param name="searchText">Search text (case-insensitive)</param>
    /// <param name="isPagingEnabled">Set false to get all results</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet("suggest")]
    [ProducesResponseType(typeof(IItemSet<SuggestRecipesResultItem>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SuggestAsync(
        [FromServices] SuggestRecipesHandler suggestHandler,
        [FromQuery] string? searchText = null,
        [FromQuery] bool isPagingEnabled = true,
        [FromQuery] int take = 8)
    {
        var request = new SuggestRecipesRequest(
            SearchText: searchText,
            IsPagingEnabled: isPagingEnabled,
            Page: 1,
            Take: take);

        return await suggestHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Get a recipe.
    /// </summary>
    /// <param name="getHandler"></param>
    /// <param name="id">The ID of the recipe to get</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetRecipeResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> GetAsync([FromServices] GetRecipeHandler getHandler, int id)
    {
        var request = new GetRecipeRequest(id);

        return await getHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Save a recipe. Will update if found, otherwise a new recipe will be created.
    /// </summary>
    /// <param name="saveHandler"></param>
    /// <param name="request">The recipe to save</param>
    [HttpPost]
    [ProducesResponseType(typeof(EntityResponse<GetRecipeResponse>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SaveAsync([FromServices] SaveRecipeHandler saveHandler, [FromBody] SaveRecipeRequest request)
    {
        return await saveHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Delete a recipe by moving it to the recycle bin.
    /// </summary>
    /// <param name="deleteHandler"></param>
    /// <param name="id">The ID of the recipe</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> DeleteAsync([FromServices] DeleteRecipeHandler deleteHandler, int id)
    {
        var request = new DeleteRecipeRequest(id);

        return await deleteHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// List the recipes in the recycle bin, most recently deleted first.
    /// </summary>
    /// <param name="listHandler"></param>
    /// <param name="isPagingEnabled">Set false to get all results</param>
    /// <param name="page">The page of results to retrieve</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet("deleted")]
    [ProducesResponseType(typeof(ListDeletedRecipesResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> ListDeletedAsync([FromServices] ListDeletedRecipesHandler listHandler, bool isPagingEnabled = true, int page = 1, int take = 30)
    {
        var request = new ListDeletedRecipesRequest(
            IsPagingEnabled: isPagingEnabled,
            Page: page,
            Take: take);

        return await listHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Permanently delete every recipe in the recycle bin.
    /// </summary>
    /// <param name="emptyHandler"></param>
    [HttpDelete("deleted")]
    [ProducesResponseType(typeof(UserMessage), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> EmptyRecycleBinAsync([FromServices] EmptyRecycleBinHandler emptyHandler)
    {
        return await emptyHandler
            .Handle(new EmptyRecycleBinRequest())
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Restore a recipe from the recycle bin.
    /// </summary>
    /// <param name="restoreHandler"></param>
    /// <param name="id">The ID of the recipe</param>
    [HttpPost("{id}/restore")]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> RestoreAsync([FromServices] RestoreRecipeHandler restoreHandler, int id)
    {
        var request = new RestoreRecipeRequest(id);

        return await restoreHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Permanently delete a recipe that is in the recycle bin.
    /// </summary>
    /// <param name="deleteHandler"></param>
    /// <param name="id">The ID of the recipe</param>
    [HttpDelete("{id}/permanent")]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> PermanentlyDeleteAsync([FromServices] PermanentlyDeleteRecipeHandler deleteHandler, int id)
    {
        var request = new PermanentlyDeleteRecipeRequest(id);

        return await deleteHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Rebuild the recipe search index.
    /// </summary>
    [HttpPost("rebuild-index")]
    [ProducesResponseType(typeof(UserMessage), 200)]
    public async Task<IActionResult> RebuildAsync([FromServices] ISearchIndexService searchIndex, CancellationToken cancellationToken)
    {
        await searchIndex.RebuildAsync(SearchIndex.Recipes, cancellationToken);

        return HttpResponder.Respond(Result.Ok(new UserMessage("Recipe index rebuilt.")));
    }
}
