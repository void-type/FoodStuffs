using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.ClientApp;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("app")]
    public class ApplicationController : Controller
    {

        public ApplicationController(HttpResponder responder, GetApplicationInfo.Handler getApplicationInfoHandler, GetApplicationInfo.Logger getApplicationInfoLogger)
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
                .Handle(new GetApplicationInfo.Request());

            return _responder.Respond(result);
        }

        private readonly HttpResponder _responder;
        private readonly GetApplicationInfo.Handler _getApplicationInfoHandler;
        private readonly GetApplicationInfo.Logger _getApplicationInfoLogger;
    }
}
