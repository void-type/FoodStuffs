using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("app")]
    public class ApplicationController : Controller
    {

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
            var result = await _getHandler
                .AddPostProcessor(_getLogger)
                .Handle(new GetWebApplicationInfo.Request());

            return _responder.Respond(result);
        }

        private readonly HttpResponder _responder;
        private readonly GetWebApplicationInfo.Handler _getHandler;
        private readonly GetWebApplicationInfo.Logger _getLogger;
    }
}
