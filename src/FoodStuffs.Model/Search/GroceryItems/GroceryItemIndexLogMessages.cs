using Microsoft.Extensions.Logging;

namespace FoodStuffs.Model.Search.GroceryItems;

internal static partial class GroceryItemIndexLogMessages
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Starting rebuild of grocery item search index.")]
    public static partial void RebuildStarted(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Finished rebuild of grocery item search index. {DocCount} documents.")]
    public static partial void RebuildFinished(ILogger logger, int docCount);
}
