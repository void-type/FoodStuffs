using FoodStuffs.Model.Actions.Core.Responder;
using FoodStuffs.Model.Actions.Core.Responses.CountedItemSet;
using FoodStuffs.Model.Actions.Core.Responses.MessageString;
using FoodStuffs.Model.Interfaces.Services.Logging;
using FoodStuffs.Model.Validation.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Web.Actions
{
    public class ActionResultResponder : ActionResponder<IActionResult>
    {
        public ActionResultResponder(ILoggingService logger)
        {
            _logger = logger;
        }

        public override void WithData<TItemType>(TItemType item, string logExtra = null)
        {
            _logger.Info(logExtra);
            Response = new ObjectResult(item);
        }

        public override void WithDataList<TItemType>(IEnumerable<TItemType> items, string logExtra = null)
        {
            var set = new CountedItemSet<TItemType> { Items = items.ToList() };

            _logger.Info(logExtra, $"Count: {set.Count}");
            Response = new ObjectResult(set) { StatusCode = 200 };
        }

        public override void WithError(string userMessage, string logExtra = null, Exception ex = null)
        {
            _logger.Error(ex, logExtra, $"ErrorUserMessage: {userMessage}");

            Response = new ObjectResult(new ErrorMessage(userMessage)) { StatusCode = 500 };
        }

        public override void WithSuccess(string userMessage, string logExtra = null)
        {
            _logger.Info(logExtra, $"SuccessUserMessage: {userMessage}");
            Response = new ObjectResult(new SuccessMessage(userMessage));
        }

        private readonly ILoggingService _logger;

        protected override void CreateValidationErrorResponse(string logExtra)
        {
            var logParams = new List<string>
                {
                    logExtra,
                    "ValidationErrors:"
                }
                .Concat(ValidationErrors.Select(error => error.ErrorMessage))
                .ToArray();

            _logger.Warn(logParams);

            var set = new CountedItemSet<IValidationError>
            {
                Items = ValidationErrors
            };

            Response = new ObjectResult(set)
            {
                StatusCode = 400
            };
        }
    }
}