using Core.Model.Actions.Chain;
using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Model.Validation;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Core.Test.Tests.Actions.Steps
{
    public class ValidateTests
    {
        [Fact]
        public void ValidateInvalid()
        {
            var responder = new SimpleActionResponder();

            var mockValidator = new Mock<IValidator<string>>();

            mockValidator.Setup(mock => mock.Validate(It.IsAny<string>()))
                .Returns(new List<ValidationError>() { new ValidationError() });

            new ActionChain(responder)
                .Execute(new Validate<string>(mockValidator.Object, ""));

            var response = responder.Response?.ValidationErrors;

            Assert.NotNull(response);
            Assert.True(responder.ResponseCreated);
            mockValidator.Verify(mock => mock.Validate(""), Times.Exactly(1));
        }

        [Fact]
        public void ValidateValid()
        {
            var responder = new SimpleActionResponder();

            var mockValidator = new Mock<IValidator<string>>();

            mockValidator.Setup(mock => mock.Validate(It.IsAny<string>()))
                .Returns(new List<ValidationError>());

            new ActionChain(responder)
                .Execute(new Validate<string>(mockValidator.Object, ""));

            var response = responder.Response?.ValidationErrors;

            Assert.Null(response);
            Assert.False(responder.ResponseCreated);
            mockValidator.Verify(mock => mock.Validate(""), Times.Exactly(1));
        }
    }
}