using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Moq;
using Xunit;

namespace Core.Test.Actions.Steps
{
    public class RespondWithItemTests
    {
        [Fact]
        public void WithItemCalled()
        {
            var mockResponder = new Mock<IActionResponder>();
            mockResponder.Setup(mock => mock.WithItem("item", "log"));

            new RespondWithItem<string>("item", "log").Execute(mockResponder.Object);

            mockResponder.Verify(mock => mock.WithItem("item", "log"), Times.Exactly(1));
        }
    }
}