using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Moq;
using Moq.Protected;
using System;
using Xunit;

namespace Core.Test.Actions.Steps
{
    public class AbstractActionStepTests
    {
        [Fact]
        public void CaptureException()
        {
            var mockResponder = new Mock<IActionResponder>
            {
                CallBase = true
            };
            mockResponder.Setup(mock => mock.WithError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Exception>()));

            var mockStep = new Mock<AbstractActionStep>
            {
                CallBase = true
            };
            mockStep.Protected().Setup("PerformStep", mockResponder.Object).Throws(new Exception("Test Exception"));

            var step = mockStep.Object;
            step.Execute(mockResponder.Object);

            mockStep.Protected().Verify("PerformStep", Times.Once(), ItExpr.IsAny<IActionResponder>());
            mockResponder.Verify(mock => mock.WithError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Exception>()), Times.Once());
        }

        [Fact]
        public void PerformStep()
        {
            var mockResponder = new Mock<IActionResponder>();

            var mockStep = new Mock<AbstractActionStep>
            {
                CallBase = true
            };
            mockStep.Protected().Setup("PerformStep", ItExpr.IsAny<IActionResponder>());

            var step = mockStep.Object;
            step.Execute(mockResponder.Object);

            mockStep.Protected().Verify("PerformStep", Times.Once(), ItExpr.IsAny<IActionResponder>());
        }
    }
}