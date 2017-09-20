using FoodStuffs.Model.Validation.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Model.Actions.Core.Responder
{
    /// <summary>
    /// Adapter for ActionSteps to use any response implementation. Retains validation errors and final response.
    /// </summary>
    public abstract class ActionResponder<TResponse> : IActionResponder where TResponse : class
    {
        public TResponse Response { get; set; }

        public bool ResponseCreated => Response != default(TResponse);

        public List<IValidationError> ValidationErrors { get; protected set; } = new List<IValidationError>();

        public bool TryWithValidationError(string logExtra = null)
        {
            if (!ValidationErrors.Any())
            {
                return false;
            }

            CreateValidationErrorResponse(logExtra);
            return true;
        }

        protected abstract void CreateValidationErrorResponse(string logExtra);

        public abstract void WithData<T>(T item, string logExtra = null);

        public abstract void WithDataList<T>(IEnumerable<T> items, string logExtra = null);

        public abstract void WithError(string userMessage, string logExtra = null, Exception ex = null);

        public abstract void WithSuccess(string userMessage, string logExtra = null);

        public void WithValidationErrors(string logExtra, params IValidationError[] newValidationErrors)
        {
            ValidationErrors.AddRange(newValidationErrors);
            CreateValidationErrorResponse(logExtra);
        }
    }
}