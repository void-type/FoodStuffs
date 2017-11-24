using Core.Model.Actions.Responder;
using Core.Model.Validation;

namespace Core.Model.Actions.Steps
{
    public class ValidateViewModel<TValidatable> : IActionStep
    {
        public ValidateViewModel(AbstractSimpleValidator<TValidatable> validator, TValidatable viewModel)
        {
            _validator = validator;
            _viewModel = viewModel;
        }

        public void Execute(IActionResponder respond)
        {
            respond.ValidationErrors.AddRange(_validator.Validate(_viewModel));

            respond.TryWithValidationError();
        }

        private readonly AbstractSimpleValidator<TValidatable> _validator;
        private readonly TValidatable _viewModel;
    }
}