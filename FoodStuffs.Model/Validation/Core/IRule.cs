using System;

namespace FoodStuffs.Model.Validation.Core
{
    public interface IRule
    {
        IValidationError ValidationError { get; }

        IRule When(Func<bool> conditionExpression);

        bool IsValid { get; }
    }
}