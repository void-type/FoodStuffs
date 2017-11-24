using System.Collections.Generic;

namespace Core.Model.Validation
{
    public interface IViewModelValidator<in TValidatable>
    {
        IEnumerable<IValidationError> Validate(TValidatable validatableEntity);
    }
}