using System;

namespace Core.Model.Validation
{
    public interface IRule
    {
        bool IsValid { get; }
        IValidationError ValidationError { get; }

        IRule When(Func<bool> conditionExpression);
    }
}