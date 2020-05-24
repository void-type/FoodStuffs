using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetInfo()
        {
            return await _getHandler
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

    internal class AppVersion
    {
        public string Version { get; }
        public bool IsPublicRelease { get; }
        public bool IsPrerelease { get; }
        public string GitCommitId { get; }
        public DateTime GitCommitDate { get; }
        public string AssemblyConfiguration { get; }

        public AppVersion(string version, bool isPublicRelease, bool isPrerelease, string gitCommitId, DateTime gitCommitDate, string assemblyConfiguration)
        {
            Version = version;
            IsPublicRelease = isPublicRelease;
            IsPrerelease = isPrerelease;
            GitCommitId = gitCommitId;
            GitCommitDate = gitCommitDate;
            AssemblyConfiguration = assemblyConfiguration;
        }
    }
}
