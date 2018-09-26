using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.ClientApp;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("app")]
    public class ApplicationController : Controller
    {

        public ApplicationController(HttpResponder responder, IApplicationInfo applicationInfo)
        {
            _responder = responder;
            _applicationInfo = applicationInfo;
        }

        [HttpGet]
        [Route("info")]
        public IActionResult GetInfo()
        {
            return _responder.Respond(_applicationInfo);
        }

        private readonly HttpResponder _responder;
        private readonly IApplicationInfo _applicationInfo;
    }
}
