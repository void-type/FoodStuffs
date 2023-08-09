using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.MealSets;

public class SaveMealSetRequestLogger : RequestLoggerAbstract<SaveMealSetRequest>
{
    public SaveMealSetRequestLogger(ILogger<SaveMealSetRequestLogger> logger) : base(logger) { }

    public override void Log(SaveMealSetRequest request)
    {
        Logger.LogInformation("Requested. MealSetId: {MealSetId}",
            request.Id);
    }
}
