using Core.Model.Actions.Chain;
using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Xunit;

namespace Core.Test.Tests.Actions
{
    public class RespondWithSuccessTests
    {
        [Fact]
        public void RespondWithSuccess()
        {
            var responder = new SimpleActionResponder();

            new ActionChain(responder)
                .Execute(new RespondWithSuccess("Congratulations!"));

            var response = responder.Response.Success;

            Assert.NotNull(response);
            Assert.True(responder.ResponseCreated);
            Assert.Equal("Congratulations!", response.Message);
        }
    }
}