using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Core.Test.Actions.Steps
{
    public class RespondWithSetTests
    {
        [Fact]
        public void WithSetCalled()
        {
            var mockResponder = new Mock<IActionResponder>();
            mockResponder.Setup(mock => mock.WithSet(new List<string> { "", "", "" }, "log"));

            new RespondWithSet<string>(new List<string> { "", "", "" }, "log").Execute(mockResponder.Object);

            mockResponder.Verify(mock => mock.WithSet(new List<string> { "", "", "" }, "log"), Times.Exactly(1));
        }
    }
}