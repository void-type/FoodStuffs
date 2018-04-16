using Core.Model.Actions.Chain;
using Core.Model.Actions.Responder;
using Core.Model.Actions.Steps;
using Core.Model.Validation;
using Xunit;

namespace Core.Test.Tests.Actions.Steps
{
    public class RespondOnValidationErrorTests
    {
        [Fact]
        public void NoValidationErrorsToRespondTo()
        {
            var responder = new SimpleActionResponder();

            new ActionChain(responder)
                .Execute(new RespondOnValidationError());

            var response = responder.Response?.ValidationErrors;

            Assert.Null(response);
            Assert.False(responder.ResponseCreated);
        }

        [Fact]
        public void RespondWithValidationError()
        {
            var responder = new SimpleActionResponder();

            responder.ValidationErrors.Add(new ValidationError());
            responder.ValidationErrors.Add(new ValidationError());

            new ActionChain(responder)
                .Execute(new RespondOnValidationError());

            var response = responder.Response.ValidationErrors;

            Assert.NotNull(response);
            Assert.True(responder.ResponseCreated);
            Assert.Equal(2, response.Count);
        }
    }
}