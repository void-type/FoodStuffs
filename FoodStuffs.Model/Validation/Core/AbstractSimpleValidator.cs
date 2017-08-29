using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Validation.Core
{
    public abstract class AbstractSimpleValidator<TValidatable> : IModelValidator<TValidatable>
    {
        protected IEnumerable<IValidationError> Errors => _rules.Where(rule => !rule.IsValid).Select(rule => rule.ValidationError);

        private readonly List<IRule> _rules = new List<IRule>();

        public IEnumerable<IValidationError> Validate(TValidatable validatable)
        {
            SetRules(validatable);
            return Errors;
        }

        protected IRule Valid(string fieldName, string errorMessage)
        {
            var rule = new Rule(fieldName, errorMessage);

            _rules.Add(rule);

            return rule;
        }

        protected abstract void SetRules(TValidatable validatable);
    }
}