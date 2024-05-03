using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.MealPlans;

public class DeleteMealPlanRequestLogger : RequestLoggerAbstract<DeleteMealPlanRequest>
{
    public DeleteMealPlanRequestLogger(ILogger<DeleteMealPlanRequestLogger> logger) : base(logger) { }

    public override void Log(DeleteMealPlanRequest request)
    {
        Logger.LogInformation("Requested. MealPlanId: {MealPlanId}",
            request.Id);
    }
}
