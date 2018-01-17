using Core.Model.Services.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Web.Services
{
    public class ActionToAspNetLoggerAdapter : ILoggingService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public ActionToAspNetLoggerAdapter(ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory.CreateLogger("Application");
        }

        public void Debug(Exception ex, params string[] messages)
        {
            var message = MakeLogString(MakeExceptionMessage(ex).Concat(messages).ToArray());
            _logger.LogDebug(message);
        }

        public void Debug(params string[] messages)
        {
            _logger.LogDebug(MakeLogString(messages));
        }

        public void Error(Exception ex, params string[] messages)
        {
            var message = MakeLogString(MakeExceptionMessage(ex).Concat(messages).ToArray());
            _logger.LogError(message);
        }

        public void Error(params string[] messages)
        {
            _logger.LogError(MakeLogString(messages));
        }

        public void Fatal(Exception ex, params string[] messages)
        {
            var message = MakeLogString(MakeExceptionMessage(ex).Concat(messages).ToArray());
            _logger.LogCritical(message);
        }

        public void Fatal(params string[] messages)
        {
            _logger.LogCritical(MakeLogString(messages));
        }

        public void Info(Exception ex, params string[] messages)
        {
            var message = MakeLogString(MakeExceptionMessage(ex).Concat(messages).ToArray());
            _logger.LogInformation(message);
        }

        public void Info(params string[] messages)
        {
            _logger.LogInformation(MakeLogString(messages));
        }

        public void Warn(Exception ex, params string[] messages)
        {
            var message = MakeLogString(MakeExceptionMessage(ex).Concat(messages).ToArray());
            _logger.LogWarning(message);
        }

        public void Warn(params string[] messages)
        {
            _logger.LogWarning(MakeLogString(messages));
        }

        private static IEnumerable<string> MakeExceptionMessage(Exception ex)
        {
            if (ex == null)
            {
                return new List<string>();
            }

            var exceptionMessages = new List<string> { "Threw Exception: " };

            do
            {
                exceptionMessages.Add($"{ex.GetType()}: {ex.Message}");
                ex = ex.InnerException;
            } while (ex != null);

            return exceptionMessages;
        }

        private string MakeLogString(params string[] messages)
        {
            var request = _httpContextAccessor.HttpContext.Request;

            var payload = string.Join(" ", messages.Where(message => !string.IsNullOrWhiteSpace(message)));

            return $"{request.Method.PadRight(8)} {request.Path.Value.PadRight(40)} {payload}";
        }
    }
}