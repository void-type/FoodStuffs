using Core.Model.Actions.Responder;
using Core.Model.Actions.Responses.File;
using System;
using System.Collections.Generic;

namespace Core.Test.Actions.Responder
{
    internal class ActionResponderStub : AbstractActionResponder<string>
    {
        public void SetResponse(string response)
        {
            Response = response;
        }

        public override void WithError(string userMessage, string logExtra = null, Exception ex = null)
        {
            throw new NotImplementedException();
        }

        public override void WithFile(IFileViewModel file, string logExtra = null)
        {
            throw new NotImplementedException();
        }

        public override void WithItem<T>(T item, string logExtra = null)
        {
            throw new NotImplementedException();
        }

        public override void WithPostSuccess(string userMessage, string id, string logExtra = null)
        {
            throw new NotImplementedException();
        }

        public override void WithSet<T>(IEnumerable<T> items, string logExtra = null)
        {
            throw new NotImplementedException();
        }

        public override void WithSuccess(string userMessage, string logExtra = null)
        {
            throw new NotImplementedException();
        }

        protected override void CreateValidationErrorResponse(string logExtra)
        {
            throw new NotImplementedException();
        }
    }
}