using Core.Model.Actions.Chain;
using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Xunit;

namespace Core.Test.Tests.Actions 
{
    public class RespondWithItemTests 
    {
        [Fact]
        public void RespondWithItem()
        {
            var responder = new SimpleActionResponder();

            new ActionChain(responder)
                .Execute(new RespondWithItem<string>("Item"));

            var response = responder.Response.Item as string;

            Assert.NotNull(response);
            Assert.True(responder.ResponseCreated);
        }

        [Fact]
        public void RespondWithNull()
        {
            var responder = new SimpleActionResponder();

            new ActionChain(responder)
                .Execute(new RespondWithItem<string>(null));

            var response = responder.Response.Item as string;

            Assert.Null(response);
            Assert.True(responder.ResponseCreated);
        }
    }
}