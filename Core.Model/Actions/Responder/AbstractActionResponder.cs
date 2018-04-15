using Core.Model.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Model.Actions.Responder
{
    /// <summary>
    /// Adapter for ActionSteps to use any response implementation.
    /// </summary>
    public abstract class AbstractActionResponder<TResponse> : IActionResponder where TResponse : class
    {
        public TResponse Response { get; protected set; }
        public bool ResponseCreated => Response != default(TResponse);
        public List<IValidationError> ValidationErrors { get; } = new List<IValidationError>();

        public bool TryWithValidationError(string logExtra = null)
        {
            if (!ValidationErrors.Any())
            {
                return false;
            }
            CreateValidationErrorResponse(logExtra);
            return true;
        }

        public abstract void WithError(string userMessage, string logExtra = null, Exception ex = null);

        public abstract void WithItem<T>(T item, string logExtra = null);

        public abstract void WithPostSuccess(string userMessage, string id, string logExtra = null);

        public abstract void WithSet<T>(IEnumerable<T> items, string logExtra = null);

        public abstract void WithSuccess(string userMessage, string logExtra = null);

        public void WithValidationErrors(string logExtra, params IValidationError[] newValidationErrors)
        {
            ValidationErrors.AddRange(newValidationErrors);
            CreateValidationErrorResponse(logExtra);
        }

        protected abstract void CreateValidationErrorResponse(string logExtra);
    }
}