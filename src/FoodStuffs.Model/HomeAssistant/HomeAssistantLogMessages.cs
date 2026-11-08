using Microsoft.Extensions.Logging;
using System.Net;

namespace FoodStuffs.Model.HomeAssistant;

internal static partial class HomeAssistantLogMessages
{
    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to get 'Remind to Thaw Meat' entity state from Home Assistant.")]
    public static partial void GetThawMeatStateFailed(ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to set 'Remind to Thaw Meat' entity state in Home Assistant.")]
    public static partial void SetThawMeatStateFailed(ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Information, Message = "Set entity state request sent to Home Assistant for entity ID: {EntityId}. Desired state: {State}. Response status code: {StatusCode}. Response content:\n{ResponseContent}")]
    public static partial void SetEntityStateRequestSent(ILogger logger, string entityId, bool state, HttpStatusCode statusCode, string responseContent);
}
