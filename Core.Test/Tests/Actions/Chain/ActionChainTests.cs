using Core.Model.Actions.Chain;
using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Moq;
using Xunit;

namespace Core.Test.Tests.Actions.Chain 
{
    public class ActionChainTests 
    {
        [Fact]
        public void ExecuteStep() {
            var mockResponder = new Mock<IActionResponder>();
            mockResponder.Setup(mock => mock.ResponseCreated).Returns(false);

            var mockStep = new Mock<IActionStep>();
            mockStep.Setup(mock => mock.Execute(mockResponder.Object));

            var chain = new ActionChain(mockResponder.Object);
            chain.Execute(mockStep.Object);

            mockStep.Verify(mock => mock.Execute(mockResponder.Object), Times.Once());
        }

        [Fact]
        public void DontExecuteBecauseResponseCreated() {
            var mockResponder = new Mock<IActionResponder>();
            mockResponder.Setup(mock => mock.ResponseCreated).Returns(true);

            var mockStep = new Mock<IActionStep>();
            mockStep.Setup(mock => mock.Execute(mockResponder.Object));

            var chain = new ActionChain(mockResponder.Object);
            chain.Execute(mockStep.Object);

            mockStep.Verify(mock => mock.Execute(mockResponder.Object), Times.Never());
        }
    }
}