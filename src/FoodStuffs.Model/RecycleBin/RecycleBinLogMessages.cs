using Microsoft.Extensions.Logging;

namespace FoodStuffs.Model.RecycleBin;

internal static partial class RecycleBinLogMessages
{
    [LoggerMessage(Level = LogLevel.Information, Message = "{Message}")]
    public static partial void Purged(ILogger logger, string message);

    [LoggerMessage(Level = LogLevel.Error, Message = "Error purging the recycle bin.")]
    public static partial void PurgeFailed(ILogger logger, Exception exception);
}
