using FoodStuffs.Model.Events;
using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Events.Recipes.Models;
using FoodStuffs.Model.Search.Recipes;
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
    /// <param name="allCategories">When true, recipes returned will match all selected categories</param>
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
        [FromQuery] bool allCategories = false,
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
            AllCategories: allCategories,
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
    /// Delete a recipe.
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
    /// Rebuild the recipe search index.
    /// </summary>
    [HttpPost("rebuild-index")]
    [ProducesResponseType(typeof(UserMessage), 200)]
    public async Task<IActionResult> RebuildAsync([FromServices] IRecipeIndexService indexService, CancellationToken cancellationToken)
    {
        await indexService.RebuildAsync(cancellationToken);

        return HttpResponder.Respond(Result.Ok(new UserMessage("Index rebuilt.")));
    }
}
