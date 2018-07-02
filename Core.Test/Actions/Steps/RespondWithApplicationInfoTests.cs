using Core.Model.Actions.Responder;
using Core.Services.Action;
using Core.Services.ClientApp;
using Moq;
using Xunit;

namespace Core.Test.Actions.Steps
{
    public class RespondWithApplicationInfoTests
    {
        [Fact]
        public void RespondWithAppInfo()
        {
            IApplicationInfo appInfo = null;

            var mockResponder = new Mock<IActionResponder>();
            mockResponder
                .Setup(mock => mock.WithItem(It.IsAny<IApplicationInfo>(), It.IsAny<string>()))
                .Callback((IApplicationInfo info, string log) =>
                {
                    appInfo = info;
                });

            new RespondWithApplicationInfo("AppName", "UserName", "request-token").Execute(mockResponder.Object);

            mockResponder.Verify(mock => mock.WithItem(It.IsAny<ApplicationInfo>(), null), Times.Exactly(1));

            Assert.Equal("AppName", appInfo.ApplicationName);
            Assert.Equal("UserName", appInfo.UserName);
            Assert.Equal("request-token", appInfo.AntiforgeryToken);
        }
    }
}
