using System;

namespace Core.Model.Validation
{
    /// <summary>
    /// A rule for validating an entity.
    /// </summary>
    public class Rule : IRule
    {
        /// <summary>
        /// If true, the validator will not return this error.
        /// </summary>
        public bool IsSuppressed { get; private set; }

        /// <summary>
        /// Returns false if the rule was violated.
        /// </summary>
        public bool IsValid { get; private set; } = true;

        /// <summary>
        /// The UI-friendly error that can be displayed if the rule is violated.
        /// </summary>
        public IValidationError ValidationError { get; }

        /// <summary>
        /// Construct a new rule by creating it's matching validation error.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="errorMessage"></param>
        public Rule(string fieldName, string errorMessage)
        {
            ValidationError = new ValidationError(errorMessage, fieldName);
        }

        public IRule Suppress(Func<bool> conditionExpression)
        {
            if (IsSuppressed == false && conditionExpression.Invoke())
            {
                IsSuppressed = true;
            }

            return this;
        }

        /// <summary>
        /// Build a boolean-returning expression to invoke as a rule against the model. Multiple "when" statements can be chained together to make a
        /// more complex rule.
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <returns></returns>
        public IRule When(Func<bool> conditionExpression)
        {
            // If this rule is already violated, don't reevaluate it.
            if (IsValid)
            {
                IsValid = !conditionExpression.Invoke();
            }

            return this;
        }
    }
}