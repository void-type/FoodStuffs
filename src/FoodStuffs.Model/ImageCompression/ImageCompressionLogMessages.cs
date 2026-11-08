using Microsoft.Extensions.Logging;

namespace FoodStuffs.Model.ImageCompression;

internal static partial class ImageCompressionLogMessages
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Skipping compression. Already webp.")]
    public static partial void SkippingCompression(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Compressed image from {OldSize} KB to {NewSize} KB.")]
    public static partial void Compressed(ILogger logger, long oldSize, long newSize);
}
