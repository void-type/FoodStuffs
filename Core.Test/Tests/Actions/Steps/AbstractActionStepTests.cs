using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Model.Validation;
using Moq;
using Moq.Protected;
using Xunit;
using System;

namespace Core.Test.Tests.Actions.Steps
{
    public class AbstractActionStepTests
    {
        [Fact]
        public void PerformStep()
        {
            var mockResponder = new Mock<IActionResponder>();

            var mockStep = new Mock<AbstractActionStep>();
            mockStep.CallBase = true;
            mockStep.Protected().Setup("PerformStep", ItExpr.IsAny<IActionResponder>());
            
            var step = mockStep.Object;
            step.Execute(mockResponder.Object);

            mockStep.Protected().Verify("PerformStep", Times.Once(), ItExpr.IsAny<IActionResponder>());
        }


        [Fact]
        public void CaptureException()
        {
            var mockResponder = new Mock<IActionResponder>();
            mockResponder.CallBase = true;
            mockResponder.Setup(mock => mock.WithError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Exception>()));

            var mockStep = new Mock<AbstractActionStep>();
            mockStep.CallBase = true;
            mockStep.Protected().Setup("PerformStep", mockResponder.Object).Throws(new Exception("Test Exception"));
            
            var step = mockStep.Object;
            step.Execute(mockResponder.Object);

            mockStep.Protected().Verify("PerformStep", Times.Once(), ItExpr.IsAny<IActionResponder>());
            mockResponder.Verify(mock => mock.WithError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Exception>()), Times.Once());
        }
    }
}