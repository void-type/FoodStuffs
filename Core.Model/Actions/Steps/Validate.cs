using Core.Model.Actions.Responder;
using Core.Model.Validation;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// Validate an entity.
    /// </summary>
    /// <typeparam name="TValidatable"></typeparam>
    public class Validate<TValidatable> : AbstractActionStep
    {
        public Validate(IValidator<TValidatable> validator, TValidatable validatableEntity, string logExtra = null)
        {
            _validator = validator;
            _validatableEntity = validatableEntity;
            _logExtra = logExtra;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.ValidationErrors.AddRange(_validator.Validate(_validatableEntity));
            respond.TryWithValidationError(_logExtra);
        }

        private readonly string _logExtra;
        private readonly TValidatable _validatableEntity;
        private readonly IValidator<TValidatable> _validator;
    }
}