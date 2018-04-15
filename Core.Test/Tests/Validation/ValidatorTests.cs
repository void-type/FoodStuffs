using Core.Model.Validation;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace Core.Test.Tests.Validations 
{
    public class ValidatorTests 
    {
        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Validator(bool isValid, bool isSuppressed, bool errorsExpectedIn)
        {
            var validator = new TestValidator(isSuppressed);

            var errors = validator.Validate(isValid).ToList();

            var errorsExpected = !(isValid || isSuppressed);

            Assert.Equal(errorsExpected, errors.Any());
        }
    }

    public class TestValidator : AbstractSimpleValidator<bool> {
        public TestValidator(bool isSuppressed)
        {
            _isSuppressed = isSuppressed;
        }

        protected override void RunRules(bool isValid) {
            Invalid("test", "test is invalid")
                .When(() => !isValid)
                .Suppress(() => _isSuppressed);
        }

        private bool _isSuppressed;
    }
}