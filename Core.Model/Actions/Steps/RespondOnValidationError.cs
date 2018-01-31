using Core.Model.Actions.Responder;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// Will respond if there are any validation errors set in the responder.
    /// </summary>
    public class RespondOnValidationError : AbstractActionStep
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