using Core.Model.Actions.Chain;
using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Model.Services.Data;
using Moq;
using Xunit;

namespace Core.Test.Tests.Actions.Steps
{
    public class SaveChangesToDataTests
    {
        [Fact]
        public void SaveChangesToData()
        {
            var responder = new SimpleActionResponder();

            var mockData = new Mock<IPersistable>();

            mockData.Setup(mock => mock.SaveChanges());

            new ActionChain(responder)
                .Execute(new SaveChangesToData(mockData.Object));

            var response = responder.Response;

            Assert.Null(response);
            Assert.False(responder.ResponseCreated);
            mockData.Verify(mock => mock.SaveChanges(), Times.Exactly(1));
        }
    }
}