using System.Collections.Generic;

namespace Core.Model.Validation
{
    public interface IValidator<in TValidatableEntity>
    {
        IEnumerable<IValidationError> Validate(TValidatableEntity validatableEntity);
    }
}