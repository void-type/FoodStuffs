using FoodStuffs.Model.Actions.Core.Steps;

namespace FoodStuffs.Model.Actions.Core.Chain
{
    public interface IActionChain
    {
        IActionChain Execute(IActionStep step);
    }
}