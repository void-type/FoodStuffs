using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Application metadata.
/// </summary>
[ApiRoute("app")]
public class ApplicationController : ControllerBase
{
    /// <summary>
    /// Get information to bootstrap the SPA client like application name and user data.
    /// </summary>
    [HttpGet]
    [Route("info")]
    [ProducesResponseType(typeof(GetWebClientInfo.WebClientInfo), 200)]
    public Task<IActionResult> GetInfo([FromServices] GetWebClientInfo.Pipeline getPipeline)
    {
        return getPipeline
            .Handle(new GetWebClientInfo.Request())
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Get the version of the application.
    /// </summary>
    [HttpGet]
    [Route("version")]
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
}

internal record AppVersion(
    string? Version,
    bool IsPublicRelease,
    bool IsPrerelease,
    string GitCommitId,
    DateTime GitCommitDate,
    string AssemblyConfiguration);
