using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Actions.Responder;
using VoidCore.Model.Actions.Responses.File;
using VoidCore.Model.Actions.Responses.Message;

namespace FoodStuffs.Test.Mocks
{
    /// <summary>
    /// A responder that sets a simple response for testing.
    /// </summary>
    public class SimpleActionResponder : AbstractActionResponder<SimpleResponse>
    {
        public override void WithError(string userMessage, string logExtra = null, Exception ex = null)
        {
            _simpleResponse.Error = new ErrorMessage()
            {
            Message = userMessage
            };
            Response = _simpleResponse;
        }

        public override void WithFile(IFileViewModel file, string logExtra = null)
        {
            _simpleResponse.File = file;
            Response = _simpleResponse;
        }

        public override void WithItem<T>(T item, string logExtra = null)
        {
            _simpleResponse.Item = item;
            Response = _simpleResponse;
        }

        public override void WithPostSuccess(string userMessage, string id, string logExtra = null)
        {
            _simpleResponse.PostSuccess = new PostSuccessMessage()
            {
            Message = userMessage,
            Id = id
            };
            Response = _simpleResponse;
        }

        public override void WithSet<T>(IEnumerable<T> items, string logExtra = null)
        {
            _simpleResponse.Set = items.Select(item =>(object) item).ToList();
            Response = _simpleResponse;
        }

        public override void WithSuccess(string userMessage, string logExtra = null)
        {
            _simpleResponse.Success = new SuccessMessage() { Message = userMessage };
            Response = _simpleResponse;
        }

        protected override void CreateValidationErrorResponse(string logExtra)
        {
            _simpleResponse.ValidationErrors.AddRange(ValidationErrors);
            Response = _simpleResponse;
        }

        private readonly SimpleResponse _simpleResponse = new SimpleResponse();
    }
}
