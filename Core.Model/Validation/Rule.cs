using System;

namespace Core.Model.Validation
{
    public class Rule : IRule
    {
        public bool IsValid { get; private set; } = true;

        public IValidationError ValidationError { get; }

        public Rule(string fieldName, string errorMessage)
        {
            ValidationError = new ValidationError(errorMessage, fieldName);
        }

        public IRule When(Func<bool> conditionExpression)
        {
            if (IsValid)
            {
                IsValid = conditionExpression.Invoke();
            }

            return this;
        }
    }
}