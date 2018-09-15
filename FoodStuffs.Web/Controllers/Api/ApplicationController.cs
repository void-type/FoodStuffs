using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.Action;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.Actions.Chain;

namespace FoodStuffs.Web.Controllers.Api
{
    [Route("api/app")]
    public class ApplicationController
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly IAntiforgery _antiforgery;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpActionResultResponder _responder;

        public ApplicationController(HttpActionResultResponder responder, ApplicationSettings applicationSettings, IAntiforgery antiforgery, IHttpContextAccessor contextAccessor)
        {
            _responder = responder;
            _applicationSettings = applicationSettings;
            _antiforgery = antiforgery;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        [Route("info")]
        public IActionResult GetInfo()
        {
            var applicationName = _applicationSettings.Name;
            var userName = _contextAccessor.HttpContext.User.Identity.Name;
            var antiforgeryRequestToken = _antiforgery.GetAndStoreTokens(_contextAccessor.HttpContext).RequestToken;

            new ActionChain(_responder)
                .Execute(new RespondWithApplicationInfo(applicationName, userName, antiforgeryRequestToken));

            return _responder.Response;
        }
    }
}
