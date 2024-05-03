using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.MealPlans;

public class SaveMealPlanRequestLogger : RequestLoggerAbstract<SaveMealPlanRequest>
{
    public SaveMealPlanRequestLogger(ILogger<SaveMealPlanRequestLogger> logger) : base(logger) { }

    public override void Log(SaveMealPlanRequest request)
    {
        Logger.LogInformation("Requested. MealPlanId: {MealPlanId}",
            request.Id);
    }
}
