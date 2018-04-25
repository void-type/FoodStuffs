using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Moq;
using Xunit;

namespace Core.Test.Actions.Steps
{
    public class RespondWithSuccessTests
    {
        [Fact]
        public void WithSetCalled()
        {
            var mockResponder = new Mock<IActionResponder>();
            mockResponder.Setup(mock => mock.WithSuccess("message", "log"));

            new RespondWithSuccess("message", "log").Execute(mockResponder.Object);

            mockResponder.Verify(mock => mock.WithSuccess("message", "log"), Times.Exactly(1));
        }
    }
}