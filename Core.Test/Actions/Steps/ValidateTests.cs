using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Model.Validation;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Core.Test.Actions.Steps
{
    public class ValidateTests
    {
        [Fact]
        public void ValidateCalled()
        {
            var validationErrors = new List<IValidationError>();

            var mockResponder = new Mock<IActionResponder>();
            mockResponder.Setup(mock => mock.TryWithValidationError("log")).Returns(true);
            mockResponder.SetupGet(mock => mock.ValidationErrors).Returns(validationErrors);

            var mockValidator = new Mock<IValidator<string>>();
            mockValidator.Setup(mock => mock.Validate("item")).Returns(new List<IValidationError>() { new ValidationError("error") });

            new Validate<string>(mockValidator.Object, "item", "log").Execute(mockResponder.Object);

            Assert.Contains(validationErrors, error => error.ErrorMessage == "error");
            mockResponder.Verify(mock => mock.TryWithValidationError("log"), Times.Exactly(1));
            mockValidator.Verify(mock => mock.Validate("item"), Times.Exactly(1));
        }
    }
}