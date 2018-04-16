using Core.Model.Actions.Responder;
using Core.Model.Validation;
using Moq;
using Moq.Protected;
using Xunit;

namespace Core.Test.Tests.Actions.Responder
{
    public class AbstractActionResponderTests
    {
        [Fact]
        public void RespondWithValidationErrors()
        {
            var mockResponder = new Mock<AbstractActionResponder<string>>();
            mockResponder.CallBase = true;
            mockResponder.Protected().Setup("CreateValidationErrorResponse", ItExpr.IsAny<string>());
            
            var responder = mockResponder.Object;
            responder.ValidationErrors.Add(new ValidationError());
            responder.TryWithValidationError("");

            mockResponder.Protected().Verify("CreateValidationErrorResponse", Times.Once(), ItExpr.IsAny<string>());
        }


        [Fact]
        public void DontRespondWithoutValidationErrors()
        {
            var mockResponder = new Mock<AbstractActionResponder<string>>();
            mockResponder.CallBase = true;
            mockResponder.Protected().Setup("CreateValidationErrorResponse", ItExpr.IsAny<string>());

            var responder = mockResponder.Object;
            responder.TryWithValidationError();

            mockResponder.Protected().Verify("CreateValidationErrorResponse", Times.Never(), ItExpr.IsAny<string>());
        }
    }
}