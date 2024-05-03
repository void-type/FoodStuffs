using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.MealPlans;

public class GetMealPlanRequestLogger : RequestLoggerAbstract<GetMealPlanRequest>
{
    public GetMealPlanRequestLogger(ILogger<GetMealPlanRequestLogger> logger) : base(logger) { }

    public override void Log(GetMealPlanRequest request)
    {
        Logger.LogInformation("Requested. MealPlanId: {MealPlanId}",
            request.Id);
    }
}
