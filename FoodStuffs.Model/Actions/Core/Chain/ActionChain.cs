using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Actions.Core.Steps;

namespace FoodStuffs.Model.Actions.Core.Chain
{
    /// <summary>
    /// ActionChain is a tool to build a series of ActionSteps.
    /// </summary>
    public class ActionChain : IActionChain
    {
        public ActionChain(IActionResponder responder)
        {
            _responder = responder;
        }

        /// <summary>
        /// Execute the chain of ActionSteps against a response. The response contains the action state and response so the chain can be rerun against
        /// a new response without needing to clear state.
        /// </summary>
        /// <param name="step"></param>
        public IActionChain Execute(IActionStep step)
        {
            if (!_responder.ResponseCreated)
            {
                step.Execute(_responder);
            }

            return this;
        }

        private readonly IActionResponder _responder;
    }
}