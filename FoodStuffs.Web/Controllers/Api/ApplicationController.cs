using Core.Model.Actions.Chain;
using Core.Model.Actions.Steps;
using Core.Services.Action;
using Core.Services.ClientApp;
using Core.Services.Configuration;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            new ActionChain(_responder)
                .Execute(new RespondWithApplicationInfo(_applicationSettings, _antiforgery, _contextAccessor));

            return _responder.Response;
        }
    }
}