using System.Collections.Generic;
using System.Linq;

namespace Core.Model.Validation
{
    /// <summary>
    /// The base for a custom entity validator.
    /// </summary>
    /// <typeparam name="TValidatableEntity"></typeparam>
    public abstract class AbstractValidator<TValidatableEntity> : IValidator<TValidatableEntity>
    {
        /// <summary>
        /// Validate the entity against the ruleset.
        /// </summary>
        /// <param name="validatable"></param>
        /// <returns></returns>
        public IEnumerable<IValidationError> Validate(TValidatableEntity validatable)
        {
            _rules = new List<IRule>();
            RunRules(validatable);
            return Errors;
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected IRule Invalid(string fieldName, string errorMessage)
        {
            var rule = new Rule(fieldName, errorMessage);
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Override this method to build the validation ruleset.
        /// </summary>
        /// <param name="validatable"></param>
        protected abstract void RunRules(TValidatableEntity validatable);

        /// <summary>
        /// A collection of rules used to validate the entity.
        /// </summary>
        private List<IRule> _rules;

        /// <summary>
        /// The list of violations against the validation rules.
        /// </summary>
        private IEnumerable<IValidationError> Errors => _rules
            .Where(rule => !rule.IsValid)
            .Where(rule => !rule.IsSuppressed)
            .Select(rule => rule.ValidationError);
    }
}