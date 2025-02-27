using FoodStuffs.Model.Events.Categories;
using FoodStuffs.Model.Events.Categories.Models;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage recipe categories.
/// </summary>
[Route(ApiRouteAttribute.BasePath + "/categories")]
public class CategoriesController : ControllerBase
{
    /// <summary>
    /// List categories. All parameters are optional and some have defaults.
    /// </summary>
    /// <param name="listHandler"></param>
    /// <param name="name">Name contains (case-insensitive)</param>
    /// <param name="isUnused">Specify to show items that have relations or no relations</param>
    /// <param name="isPagingEnabled">Set false to get all results</param>
    /// <param name="page">The page of results to retrieve</param>
    /// <param name="take">How many items in a page</param>
    [HttpGet]
    [ProducesResponseType(typeof(IItemSet<ListCategoriesResponse>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> ListAsync([FromServices] ListCategoriesHandler listHandler, string? name = null, bool? isUnused = null, bool isPagingEnabled = true, int page = 1, int take = 30)
    {
        var request = new ListCategoriesRequest(
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
    /// Get a category.
    /// </summary>
    /// <param name="getHandler"></param>
    /// <param name="id">The ID of the category to get</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetCategoryResponse), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> GetAsync([FromServices] GetCategoryHandler getHandler, int id)
    {
        var request = new GetCategoryRequest(id);

        return await getHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Save a category. Will update if found, otherwise a new item will be created.
    /// </summary>
    /// <param name="saveHandler"></param>
    /// <param name="request">The category to save</param>
    [HttpPost]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SaveAsync([FromServices] SaveCategoryHandler saveHandler, [FromBody] SaveCategoryRequest request)
    {
        return await saveHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Delete a category.
    /// </summary>
    /// <param name="deleteHandler"></param>
    /// <param name="id">The ID of the category</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> DeleteAsync([FromServices] DeleteCategoryHandler deleteHandler, int id)
    {
        var request = new DeleteCategoryRequest(id);

        return await deleteHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Add category to all recipes.
    /// </summary>
    /// <param name="saveHandler"></param>
    /// <param name="request">The category to save</param>
    [HttpPost("add-to-all-recipes")]
    [ProducesResponseType(typeof(EntityMessage<int>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> AddToAllRecipesAsync([FromServices] AddCategoryToAllRecipesHandler saveHandler, [FromBody] AddCategoryToAllRecipesRequest request)
    {
        return await saveHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }
}
