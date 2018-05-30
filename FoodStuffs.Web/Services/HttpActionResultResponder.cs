using Core.Model.Actions.Responder;
using Core.Model.Actions.Responses.File;
using Core.Model.Actions.Responses.ItemSet;
using Core.Model.Actions.Responses.Message;
using Core.Model.Services.Logging;
using Core.Model.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodStuffs.Web.Services
{
    public class HttpActionResultResponder : AbstractActionResponder<IActionResult>
    {
        public HttpActionResultResponder(ILoggingService logger)
        {
            _logger = logger;
        }

        public override void WithError(string userMessage, string logExtra = null, Exception ex = null)
        {
            _logger.Error(ex, logExtra, $"ErrorUserMessage: {userMessage}");
            Response = new ObjectResult(
                    new ErrorMessage()
                    {
                        Message = userMessage
                    })
            {
                StatusCode = 500
            };
        }

        public override void WithFile(IFileViewModel file, string logExtra = null)
        {
            var response = new FileContentResult(Encoding.UTF8.GetBytes(file.Content), "application/force-download")
            {
                FileDownloadName = file.Name
            };

            _logger.Info(logExtra, $"FileName: {file.Name}");

            Response = response;
        }

        public override void WithItem<TItemType>(TItemType item, string logExtra = null)
        {
            _logger.Info(logExtra);
            Response = new ObjectResult(item);
        }

        public override void WithPostSuccess(string userMessage, string id, string logExtra = null)
        {
            _logger.Info(logExtra, $"SuccessUserMessage: {userMessage}, EntityId: {id}");
            Response = new ObjectResult(
                new PostSuccessMessage()
                {
                    Message = userMessage,
                    Id = id
                });
        }

        public override void WithSet<TItemType>(IEnumerable<TItemType> items, string logExtra = null)
        {
            var set = new CountedItemSet<TItemType> { Items = items.ToList() };

            _logger.Info($"Count: {set.Count}", logExtra);
            Response = new ObjectResult(set) { StatusCode = 200 };
        }

        public override void WithSuccess(string userMessage, string logExtra = null)
        {
            _logger.Info(logExtra, $"SuccessUserMessage: {userMessage}");
            Response = new ObjectResult(new SuccessMessage()
            {
                Message = userMessage
            });
        }

        protected override void CreateValidationErrorResponse(string logExtra)
        {
            var logParams = new List<string> {
                logExtra,
                "ValidationErrors: "
            }
            .Concat(ValidationErrors.Select(error => error.ErrorMessage))
            .ToArray();

            _logger.Warn(logParams);

            var set = new CountedItemSet<IValidationError>()
            {
                Items = ValidationErrors
            };
            Response = new ObjectResult(set) { StatusCode = 400 };
        }

        private readonly ILoggingService _logger;
    }
}