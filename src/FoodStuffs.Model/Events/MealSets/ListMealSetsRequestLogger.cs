using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.MealSets;

public class ListMealSetsRequestLogger : RequestLoggerAbstract<ListMealSetsRequest>
{
    public ListMealSetsRequestLogger(ILogger<ListMealSetsRequestLogger> logger) : base(logger) { }

    public override void Log(ListMealSetsRequest request)
    {
        Logger.LogInformation("Requested. NameSearch: {NameSearch} RequestIsPagingEnabled: {IsPagingEnabled} RequestPage: {Page} RequestTake: {Take}",
            request.NameSearch,
            request.IsPagingEnabled,
            request.Page,
            request.Take);
    }
}
