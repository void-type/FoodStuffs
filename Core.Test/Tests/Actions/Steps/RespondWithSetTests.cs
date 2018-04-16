using Core.Model.Actions.Chain;
using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using System.Collections.Generic;
using Xunit;

namespace Core.Test.Tests.Actions.Steps
{
    public class RespondWithSetTests
    {
        [Fact]
        public void RespondWithEmptySet()
        {
            var responder = new SimpleActionResponder();

            new ActionChain(responder)
                .Execute(new RespondWithSet<string>(new List<string>()));

            var response = responder.Response.Set;

            Assert.NotNull(response);
            Assert.True(responder.ResponseCreated);
            Assert.Empty(response);
        }

        [Fact]
        public void RespondWithSet()
        {
            var responder = new SimpleActionResponder();

            var set = new List<string> { "test", "test" };

            new ActionChain(responder)
                .Execute(new RespondWithSet<string>(set));

            var response = responder.Response.Set;

            Assert.NotNull(response);
            Assert.True(responder.ResponseCreated);
            Assert.Equal(2, response.Count);
        }
    }
}