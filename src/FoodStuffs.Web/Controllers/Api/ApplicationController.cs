using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Domain;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("app")]
    public class ApplicationController : ControllerBase
    {
        private readonly GetWebClientInfo.Handler _getHandler;
        private readonly GetWebClientInfo.Logger _getLogger;

        public ApplicationController(GetWebClientInfo.Handler getHandler, GetWebClientInfo.Logger getLogger)
        {
            _getHandler = getHandler;
            _getLogger = getLogger;
        }

        [HttpGet]
        [Route("info")]
        public Task<IActionResult> GetInfo()
        {
            return _getHandler
                .AddPostProcessor(_getLogger)
                .Handle(new GetWebClientInfo.Request())
                .MapAsync(HttpResponder.Respond);
        }

        [HttpGet]
        [Route("version")]
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
}
