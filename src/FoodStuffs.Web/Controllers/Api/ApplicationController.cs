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
    }
}
