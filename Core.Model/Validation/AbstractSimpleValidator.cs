using System.Collections.Generic;
using System.Linq;

namespace Core.Model.Validation
{
    public abstract class AbstractSimpleValidator<TValidatableEntity> : IValidator<TValidatableEntity>
    {
        /// <summary>
        /// Check if the entity is valid following the rules created by the implementation.
        /// </summary>
        /// <param name="validatable"></param>
        /// <returns></returns>
        public IEnumerable<IValidationError> Validate(TValidatableEntity validatable)
        {
            _rules = new List<IRule>();
            SetRules(validatable);
            return Errors;
        }

        /// <summary>
        /// A method used during implementation to begin creating a rule. Rule creation starts with field name and error message strings.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected IRule InValid(string fieldName, string errorMessage)
        {
            var rule = new Rule(fieldName, errorMessage);

            _rules.Add(rule);

            return rule;
        }

        /// <summary>
        /// The implementation will override this method to build the validtion ruleset. This will be called each time the entity is validated, thus
        /// the validator can be used on multiple entites.
        /// </summary>
        /// <param name="validatable"></param>
        protected abstract void SetRules(TValidatableEntity validatable);

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