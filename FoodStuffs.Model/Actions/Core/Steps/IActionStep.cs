using FoodStuffs.Model.Actions.Core.Responder;

namespace FoodStuffs.Model.Actions.Core.Steps
{
    /// <summary>
    /// A single step in an action chain.
    /// </summary>
    public interface IActionStep
    {
        void Execute(IActionResponder respond);
    }
}