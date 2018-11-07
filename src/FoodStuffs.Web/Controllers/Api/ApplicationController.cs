using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.Attributes;
using VoidCore.AspNet.ClientApp;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("app")]
    public class ApplicationController : Controller
    {

        public ApplicationController(HttpResponder responder, GetWebApplicationInfo.Handler getApplicationInfoHandler, GetWebApplicationInfo.Logger getApplicationInfoLogger)
        {
            _responder = responder;
            _getApplicationInfoHandler = getApplicationInfoHandler;
            _getApplicationInfoLogger = getApplicationInfoLogger;
        }

        [HttpGet]
        [Route("info")]
        public IActionResult GetInfo()
        {
            var result = _getApplicationInfoHandler
                .AddPostProcessor(_getApplicationInfoLogger)
                .Handle(new GetWebApplicationInfo.Request());

            return _responder.Respond(result);
        }

        private readonly HttpResponder _responder;
        private readonly GetWebApplicationInfo.Handler _getApplicationInfoHandler;
        private readonly GetWebApplicationInfo.Logger _getApplicationInfoLogger;
    }
}
