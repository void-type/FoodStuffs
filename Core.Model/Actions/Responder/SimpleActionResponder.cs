using Core.Model.Actions.Responses;
using Core.Model.Actions.Responses.MessageString;
using Core.Model.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Model.Actions.Responder
{
    /// <summary>
    /// Used for testing. Holds a simple response that can be watched for any action outputs.
    /// </summary>
    public class SimpleActionResponder : AbstractActionResponder<SimpleResponse>
    {
        public SimpleActionResponder(ILoggingService logger) : base(logger)
        {
        }

        public override void WithData<T>(T item, string logExtra = null)
        {
            _simpleResponse.DataItem = item;
            Response = _simpleResponse;
        }

        public override void WithDataList<T>(IEnumerable<T> items, string logExtra = null)
        {
            _simpleResponse.DataList = items.Select(item => (object)item).ToList();
            Response = _simpleResponse;
        }

        public override void WithError(string userMessage, string logExtra = null, Exception ex = null)
        {
            _simpleResponse.Error = new ErrorMessage(userMessage);
            Response = _simpleResponse;
        }

        public override void WithSuccess(string userMessage, string logExtra = null)
        {
            _simpleResponse.Success = new SuccessMessage(userMessage);
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