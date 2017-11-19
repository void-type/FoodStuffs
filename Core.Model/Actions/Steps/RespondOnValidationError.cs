using Core.Model.Actions.Responder;

namespace Core.Model.Actions.Steps
{
    public class RespondOnValidationError : ActionStep
    {
        public RespondOnValidationError(string logExtra = null)
        {
            _logExtra = logExtra;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.TryWithValidationError(_logExtra);
        }

        private readonly string _logExtra;
    }
}