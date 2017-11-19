using Core.Model.Actions.Responder;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// A single step in an action chain.
    /// </summary>
    public interface IActionStep
    {
        void Execute(IActionResponder respond);
    }
}