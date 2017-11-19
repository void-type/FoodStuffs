using Core.Model.Actions.Steps;

namespace Core.Model.Actions.Chain
{
    public interface IActionChain
    {
        IActionChain Execute(IActionStep step);
    }
}