using Core.Model.Actions.Responder;
using Core.Model.Actions.Responses.CountedItemSet;
using Core.Model.Actions.Responses.MessageString;
using Core.Model.Services.Logging;
using Core.Model.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Web.Services.Actions
{
  public class HttpActionResultResponder : AbstractActionResponder<IActionResult>
  {
    public HttpActionResultResponder(ILoggingService logger) : base(logger)
    {
    }

    public override void WithData<TItemType>(TItemType item, string logExtra = null)
    {
      Log.Info(logExtra);
      Response = new ObjectResult(item);
    }

    public override void WithDataList<TItemType>(IEnumerable<TItemType> items, string logExtra = null)
    {
      var set = new CountedItemSet<TItemType> { Items = items.ToList() };

      Log.Info(logExtra, $"Count: {set.Count}");
      Response = new ObjectResult(set) { StatusCode = 200 };
    }

    public override void WithError(string userMessage, string logExtra = null, Exception ex = null)
    {
      Log.Error(ex, logExtra, $"ErrorUserMessage: {userMessage}");

      Response = new ObjectResult(new ErrorMessage(userMessage)) { StatusCode = 500 };
    }

    public override void WithSuccess(string userMessage, string logExtra = null)
    {
      Log.Info(logExtra, $"SuccessUserMessage: {userMessage}");
      Response = new ObjectResult(new SuccessMessage(userMessage));
    }

    protected override void CreateValidationErrorResponse(string logExtra)
    {
      var logParams = new List<string>
                {
                    logExtra,
                    "ValidationErrors:"
                }
          .Concat(ValidationErrors.Select(error => error.ErrorMessage))
          .ToArray();

      Log.Warn(logParams);

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
