using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Search;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage recipes.
/// </summary>
[Route("api/recipes")]
public class RecipesController : ControllerBase
{
    /// <summary>
    /// Rebuild the recipe search index.
    /// </summary>
    [Route("rebuild-index")]
    [HttpPost]
    [ProducesResponseType(typeof(UserMessage), 200)]
    public async Task<IActionResult> Rebuild([FromServices] IRecipeIndexService indexService, CancellationToken cancellationToken)
    {
        await indexService.Rebuild(cancellationToken);

        return HttpResponder.Respond(Result.Ok(new UserMessage("Index rebuilt.")));
    }

    /// <summary>
    /// Search for recipes using the following criteria. All are optional and some have defaults.
    /// </summary>
    /// <param name="searchPipeline"></param>
    /// <param name="name">Name contains (case-insensitive)</param>
    /// <param name="categories">Category IDs to filter on</param>
    /// <param name="isForMealPlanning">If the recipes should be enabled for meal planning</param>
    /// <param name="sortBy">Field name to sort by (case-insensitive). Options are: newest, oldest, a-z, z-a, random. Default if empty is search score.</param>
    /// <param name="randomSortSeed">Give a seed for stable random sorting. By default is stable for 24 hours on the server.</param>
    /// <param name="isPagingEnabled">False for all results</param>
    /// <param name="page">The page of results to retrieve</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet]
    [ProducesResponseType(typeof(RecipeSearchResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public Task<IActionResult> Search([FromServices] SearchRecipesPipeline searchPipeline, string? name = null, int[]? categories = null,
        bool? isForMealPlanning = null, string? sortBy = null, string? randomSortSeed = null, bool isPagingEnabled = true, int page = 1, int take = 30)
    {
        var request = new RecipeSearchRequest(
            NameSearch: name,
            CategoryIds: categories,
            IsForMealPlanning: isForMealPlanning,
            SortBy: sortBy,
            RandomSortSeed: randomSortSeed,
            IsPagingEnabled: isPagingEnabled,
            Page: page,
            Take: take);

        return searchPipeline
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Get a recipe.
    /// </summary>
    /// <param name="getPipeline"></param>
    /// <param name="id">The ID of the recipe to get</param>
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
