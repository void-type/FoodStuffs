using Core.Model.Actions.Responder;
using Core.Model.Validation;
using Moq;
using Moq.Protected;
using Xunit;

namespace Core.Test.Actions.Responder
{
    public class AbstractActionResponderTests
    {
        [Fact]
        public void DontRespondWithoutValidationErrors()
        {
            var mockResponder = new Mock<AbstractActionResponder<string>>
            {
                CallBase = true
            };
            mockResponder.Protected().Setup("CreateValidationErrorResponse", ItExpr.IsAny<string>());

            var responder = mockResponder.Object;
            responder.TryWithValidationError();

            mockResponder.Protected().Verify("CreateValidationErrorResponse", Times.Never(), ItExpr.IsAny<string>());
        }

        [Fact]
        public void RespondWithValidationErrors()
        {
            var mockResponder = new Mock<AbstractActionResponder<string>>
            {
                CallBase = true
            };
            mockResponder.Protected().Setup("CreateValidationErrorResponse", ItExpr.IsAny<string>());

            var responder = mockResponder.Object;
            responder.ValidationErrors.Add(new ValidationError());
            responder.TryWithValidationError("");

            mockResponder.Protected().Verify("CreateValidationErrorResponse", Times.Once(), ItExpr.IsAny<string>());
        }

        [Fact]
        public void ResponseCreated()
        {
            var responder = new ActionResponderStub();

            responder.SetResponse("");

            Assert.True(responder.ResponseCreated);
        }

        [Fact]
        public void ResponseNotCreated()
        {
            var responder = new ActionResponderStub();

            responder.SetResponse(null);

            Assert.False(responder.ResponseCreated);
        }
    }
}