using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Services.Configuration;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace Core.Services.ClientApp
{
    public class RespondWithApplicationInfo : AbstractActionStep
    {
        private readonly ApplicationSettings _settings;
        private readonly IAntiforgery _antiforgery;
        private readonly IHttpContextAccessor _contextAccessor;

        public RespondWithApplicationInfo(ApplicationSettings settings, IAntiforgery antiforgery, IHttpContextAccessor contextAccessor)
        {
            _antiforgery = antiforgery;
            _contextAccessor = contextAccessor;
            _settings = settings;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var info = new ApplicationInfo
            {
                ApplicationName = _settings.Name,
                UserName = _contextAccessor.HttpContext.User.Identity.Name,
                AntiforgeryToken = _antiforgery.GetAndStoreTokens(_contextAccessor.HttpContext).RequestToken
            };

            respond.WithItem(info);
        }
    }
}