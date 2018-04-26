using System.Linq;
using Xunit;

namespace Core.Test.Validation
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

            var errorExpected = !(isValid || isSuppressed);

            Assert.Equal(errorExpected ? 1 : 0, errors.Count());
        }
    }
}