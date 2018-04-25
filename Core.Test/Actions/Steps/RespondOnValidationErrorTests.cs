using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Moq;
using Xunit;

namespace Core.Test.Actions.Steps
{
    public class RespondOnValidationErrorTests
    {
        [Fact]
        public void TryWithValidationErrorCalled()
        {
            var mockResponder = new Mock<IActionResponder>();
            mockResponder.Setup(mock => mock.TryWithValidationError("")).Returns(true);

            new RespondOnValidationError("").Execute(mockResponder.Object);

            mockResponder.Verify(mock => mock.TryWithValidationError(""), Times.Exactly(1));
        }
    }
}