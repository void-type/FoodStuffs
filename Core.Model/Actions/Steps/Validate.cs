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
        public Validate(AbstractSimpleValidator<TValidatable> validator, TValidatable validatableEntity)
        {
            _validator = validator;
            _validatableEntity = validatableEntity;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.ValidationErrors.AddRange(_validator.Validate(_validatableEntity));
            respond.TryWithValidationError();
        }

        private readonly TValidatable _validatableEntity;
        private readonly AbstractSimpleValidator<TValidatable> _validator;
    }
}