using Core.Model.Actions.Responder;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// Respond with a success message.
    /// </summary>
    public class RespondWithSuccess : AbstractActionStep
    {
        public RespondWithSuccess(string message, string logExtra = null)
        {
            _message = message;
            _logExtra = logExtra;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.WithSuccess(_message, _logExtra);
        }

        private readonly string _logExtra;
        private readonly string _message;
    }
}