using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Validation.Core;

namespace FoodStuffs.Model.Actions.Core.Steps
{
    public class Validate<TValidatable> : IActionStep
    {
        private readonly AbstractSimpleValidator<TValidatable> _validator;
        private readonly TValidatable _viewModel;

        public Validate(AbstractSimpleValidator<TValidatable> validator, TValidatable viewModel)
        {
            _validator = validator;
            _viewModel = viewModel;
        }

        public void Execute(IActionResponder respond)
        {
            respond.ValidationErrors.AddRange(_validator.Validate(_viewModel));

            respond.TryWithValidationError();
        }
    }
}