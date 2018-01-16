using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;

namespace Core.Model.Actions.Chain
{
    /// <summary>
    /// ActionChain injects the responder into each action step and stops execution when a response is set.
    /// </summary>
    public class ActionChain : IActionChain
    {
        public ActionChain(IActionResponder responder)
        {
            _responder = responder;
        }

        /// <summary>
        /// If the response is not already created, perform the next step.
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