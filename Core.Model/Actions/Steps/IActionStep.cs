using Core.Model.Actions.Responder;

namespace Core.Model.Actions.Steps
{
    public interface IActionStep
    {
        void Execute(IActionResponder respond);
    }
}