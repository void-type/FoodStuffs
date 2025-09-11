using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.Recipes;
using FoodStuffs.Web.Models;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Application metadata.
/// </summary>
[Route(ApiRouteAttribute.BasePath + "/app")]
public class AppController : ControllerBase
{
    /// <summary>
    /// Get information to bootstrap the SPA client like application name and user data.
    /// </summary>
    [HttpGet("info")]
    [ProducesResponseType(typeof(GetWebClientInfo.WebClientInfo), 200)]
    public async Task<IActionResult> GetInfoAsync([FromServices] GetWebClientInfo.Pipeline getPipeline)
    {
        return await getPipeline
            .Handle(new GetWebClientInfo.Request())
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Get the version of the application.
    /// </summary>
    [HttpGet("version")]
    [ProducesResponseType(typeof(AppVersion), 200)]
    public IActionResult GetVersion()
    {
        return new AppVersion(
                ThisAssembly.AssemblyInformationalVersion.Split('+').FirstOrDefault(),
                ThisAssembly.IsPublicRelease,
                ThisAssembly.IsPrerelease,
                ThisAssembly.GitCommitId,
                ThisAssembly.GitCommitDate,
                ThisAssembly.AssemblyConfiguration)
            .Map(HttpResponder.Respond);
    }

    /// <summary>
    /// Rebuild the search indexes.
    /// </summary>
    [HttpPost("rebuild-indexes")]
    [ProducesResponseType(typeof(UserMessage), 200)]
    public async Task<IActionResult> RebuildAsync([FromServices] IRecipeIndexService recipeIndexService, IGroceryItemIndexService groceryItemIndexService, CancellationToken cancellationToken)
    {
        await recipeIndexService.RebuildAsync(cancellationToken);
        await groceryItemIndexService.RebuildAsync(cancellationToken);

        return HttpResponder.Respond(Result.Ok(new UserMessage("Search indexes rebuilt.")));
    }
}
