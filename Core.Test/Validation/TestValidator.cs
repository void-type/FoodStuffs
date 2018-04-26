using Core.Model.Validation;

namespace Core.Test.Validation
{
    internal class TestValidator : AbstractValidator<bool>
    {
        public TestValidator(bool isSuppressed)
        {
            _isSuppressed = isSuppressed;
        }

        protected override void RunRules(bool isValid)
        {
            Invalid("test", "test is invalid")
                .When(() => !isValid)
                .ExceptWhen(() => _isSuppressed);
        }

        private readonly bool _isSuppressed;
    }
}