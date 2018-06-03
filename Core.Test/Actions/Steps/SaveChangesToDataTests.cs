using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Model.Data;
using Moq;
using Xunit;

namespace Core.Test.Actions.Steps
{
    public class SaveChangesToDataTests
    {
        [Fact]
        public void WithSetCalled()
        {
            var mockResponder = new Mock<IActionResponder>();
            var mockData = new Mock<IPersistable>();
            mockData.Setup(mock => mock.SaveChanges());

            new SaveChangesToData(mockData.Object).Execute(mockResponder.Object);

            mockData.Verify(mock => mock.SaveChanges(), Times.Exactly(1));
        }
    }
}