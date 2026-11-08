using Microsoft.Extensions.Logging;

namespace FoodStuffs.Model.Events.Images;

internal static partial class ImageLogMessages
{
    [LoggerMessage(Level = LogLevel.Error, Message = "Exception thrown while compressing image.")]
    public static partial void CompressionFailed(ILogger logger, Exception exception);
}
