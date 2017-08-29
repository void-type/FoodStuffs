using System.Collections.Generic;

namespace FoodStuffs.Model.Validation.Core
{
    public interface IModelValidator<in TValidatable>
    {
        IEnumerable<IValidationError> Validate(TValidatable validatableEntity);
    }
}