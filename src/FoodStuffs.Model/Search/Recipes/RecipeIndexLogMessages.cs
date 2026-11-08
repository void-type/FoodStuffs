using Microsoft.Extensions.Logging;

namespace FoodStuffs.Model.Search.Recipes;

internal static partial class RecipeIndexLogMessages
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Starting rebuild of recipe search index.")]
    public static partial void RebuildStarted(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Finished rebuild of recipe search index. {DocCount} documents.")]
    public static partial void RebuildFinished(ILogger logger, int docCount);
}
