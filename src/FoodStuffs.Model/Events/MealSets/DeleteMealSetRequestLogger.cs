using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.MealSets;

public class DeleteMealSetRequestLogger : RequestLoggerAbstract<DeleteMealSetRequest>
{
    public DeleteMealSetRequestLogger(ILogger<DeleteMealSetRequestLogger> logger) : base(logger) { }

    public override void Log(DeleteMealSetRequest request)
    {
        Logger.LogInformation("Requested. MealSetId: {MealSetId}",
            request.Id);
    }
}
