using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Domain;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("app")]
    public class ApplicationController : Controller
    {
        private readonly HttpResponder _responder;
        private readonly GetWebApplicationInfo.Handler _getHandler;
        private readonly GetWebApplicationInfo.Logger _getLogger;

        public ApplicationController(HttpResponder responder, GetWebApplicationInfo.Handler getHandler, GetWebApplicationInfo.Logger getLogger)
        {
            _responder = responder;
            _getHandler = getHandler;
            _getLogger = getLogger;
        }

        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> GetInfo()
        {
            return await _getHandler
                .AddPostProcessor(_getLogger)
                .Handle(new GetWebApplicationInfo.Request())
                .MapAsync(_responder.Respond);
        }
    }
}
