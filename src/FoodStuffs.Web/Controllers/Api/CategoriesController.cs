using FoodStuffs.Model.Events.Categories;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage recipes.
/// </summary>
[Route(ApiRouteAttribute.BasePath + "/categories")]
public class CategoriesController : ControllerBase
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
    [ProducesResponseType(typeof(IItemSet<ListCategoriesResponse>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> Search([FromServices] ListCategoriesPipeline listPipeline, string? name = null, bool isPagingEnabled = true, int page = 1, int take = 30)
    {
        var request = new ListCategoriesRequest(
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
}
