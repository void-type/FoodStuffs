using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.MealSets;

public class GetMealSetRequestLogger : RequestLoggerAbstract<GetMealSetRequest>
{
    public GetMealSetRequestLogger(ILogger<GetMealSetRequestLogger> logger) : base(logger) { }

    public override void Log(GetMealSetRequest request)
    {
        Logger.LogInformation("Requested. MealSetId: {MealSetId}",
            request.Id);
    }
}
