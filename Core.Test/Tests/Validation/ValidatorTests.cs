using Core.Model.Validation;
using System.Linq;
using Xunit;

namespace Core.Test.Tests.Validation
{
    public class ValidatorTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void Validator(bool isValid, bool isSuppressed)
        {
            var validator = new TestValidator(isSuppressed);

            var errors = validator.Validate(isValid).ToList();

            var errorsExpected = !(isValid || isSuppressed);

            Assert.Equal(errorsExpected, errors.Any());
        }
    }

    internal class TestValidator : AbstractSimpleValidator<bool>
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