using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Services.ClientApp;

namespace Core.Services.Action
{
    public class RespondWithApplicationInfo : AbstractActionStep
    {
        public RespondWithApplicationInfo(string applicationName, string userName, string antiforgeryRequestToken)
        {
            _applicationName = applicationName;
            _userName = userName;
            _antiforgeryRequestToken = antiforgeryRequestToken;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var info = new ApplicationInfo
            {
                ApplicationName = _applicationName,
                UserName = _userName,
                AntiforgeryToken = _antiforgeryRequestToken
            };

            respond.WithItem(info);
        }

        private readonly string _antiforgeryRequestToken;
        private readonly string _applicationName;
        private readonly string _userName;
    }
}
