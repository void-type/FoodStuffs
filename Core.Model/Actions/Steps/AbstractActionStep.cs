using Core.Model.Actions.Responder;
using System;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// A single step in an action chain. Encapsulates some model action.
    /// </summary>
    public abstract class AbstractActionStep : IActionStep
    {
        public void Execute(IActionResponder respond)
        {
            try
            {
                PerformStep(respond);
            }
            catch (Exception ex)
            {
                respond.WithError("There was a problem processing your request.", $"StepName: {GetType()}", ex);
            }
        }

        protected abstract void PerformStep(IActionResponder respond);
    }
}