using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.MealPlans;

public class ListMealPlansRequestLogger : RequestLoggerAbstract<ListMealPlansRequest>
{
    public ListMealPlansRequestLogger(ILogger<ListMealPlansRequestLogger> logger) : base(logger) { }

    public override void Log(ListMealPlansRequest request)
    {
        Logger.LogInformation("Requested. RequestIsPagingEnabled: {IsPagingEnabled} RequestPage: {Page} RequestTake: {Take}",
            request.IsPagingEnabled,
            request.Page,
            request.Take);
    }
}
