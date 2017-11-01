using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Validation.Core
{
    public abstract class AbstractSimpleValidator<TValidatable> : IModelValidator<TValidatable>
    {
        public IEnumerable<IValidationError> Validate(TValidatable validatable)
        {
            SetRules(validatable);
            return Errors;
        }

        protected IEnumerable<IValidationError> Errors => _rules.Where(rule => !rule.IsValid)
            .Select(rule => rule.ValidationError);

        protected abstract void SetRules(TValidatable validatable);

        protected IRule Valid(string fieldName, string errorMessage)
        {
            var rule = new Rule(fieldName, errorMessage);

            _rules.Add(rule);

            return rule;
        }

        private readonly List<IRule> _rules = new List<IRule>();
    }
}