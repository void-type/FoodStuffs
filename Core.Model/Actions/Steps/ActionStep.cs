using Core.Model.Actions.Responder;
using System;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// A single step in an action chain.
    /// </summary>
    public abstract class ActionStep : IActionStep
    {
        public void Execute(IActionResponder respond)
        {
            try
            {
                PerformStep(respond);
            }
            catch (Exception ex)
            {
                respond.WithError("There was a problem processing your request.", null, ex);
            }
        }

        protected abstract void PerformStep(IActionResponder respond);
    }
}