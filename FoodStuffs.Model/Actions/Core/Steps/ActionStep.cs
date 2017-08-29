using FoodStuffs.Model.Actions.Core.Responder;
using System;

namespace FoodStuffs.Model.Actions.Core.Steps
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
                respond.WithError("There was a problem processing your request. Contact Application Services for further assistance.", null, ex);
            }
        }

        protected abstract void PerformStep(IActionResponder respond);
    }
}