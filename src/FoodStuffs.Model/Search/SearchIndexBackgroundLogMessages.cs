using Microsoft.Extensions.Logging;

namespace FoodStuffs.Model.Search;

internal static partial class SearchIndexBackgroundLogMessages
{
    [LoggerMessage(Level = LogLevel.Error, Message = "Error processing deferred search index action for {IndexName} with {Count} entities.")]
    public static partial void DeferredActionFailed(ILogger logger, Exception exception, SearchIndex indexName, int count);
}
