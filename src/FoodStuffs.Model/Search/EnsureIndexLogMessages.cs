using Microsoft.Extensions.Logging;

namespace FoodStuffs.Model.Search;

internal static partial class EnsureIndexLogMessages
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "One or more index folders are missing on startup. Rebuilding.")]
    public static partial void IndexFoldersMissing(ILogger logger);

    [LoggerMessage(Level = LogLevel.Error, Message = "Recipe index test failed. Index may be corrupted. Rebuilding.")]
    public static partial void RecipeIndexTestFailed(ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Error, Message = "Grocery items index test failed. Index may be corrupted. Rebuilding.")]
    public static partial void GroceryItemIndexTestFailed(ILogger logger, Exception exception);
}
