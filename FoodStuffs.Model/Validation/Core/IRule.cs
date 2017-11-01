using System;

namespace FoodStuffs.Model.Validation.Core
{
    public interface IRule
    {
        bool IsValid { get; }
        IValidationError ValidationError { get; }

        IRule When(Func<bool> conditionExpression);
    }
}