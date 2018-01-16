using Core.Model.Actions.Responder;
using Core.Model.Actions.Responses.CountedItemSet;
using Core.Model.Actions.Responses.MessageString;
using Core.Model.Services.Logging;
using Core.Model.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Web.Services
{
  public class HttpActionResultResponder : AbstractActionResponder<IActionResult>
  {
    private readonly ILoggingService _logger;

    public HttpActionResultResponder(ILoggingService logger)
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

      _logger.Info($"Count: {set.Count}", logExtra);
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

    protected override void CreateValidationErrorResponse(string logExtra)
    {
      var logParams = new List<string>()
      {
        logExtra,
        "ValidationErrors: "
      }
      .Concat(ValidationErrors.Select(error => error.ErrorMessage))
      .ToArray();

      _logger.Warn(logParams);

      var set = new CountedItemSet<IValidationError>(ValidationErrors);

      Response = new ObjectResult(set) { StatusCode = 400 };
    }
  }
}
