using System.Collections.Generic;

namespace Core.Model.Validation
{
    public interface IModelValidator<in TValidatable>
    {
        IEnumerable<IValidationError> Validate(TValidatable validatableEntity);
    }
}