using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses;

namespace FoodStuffs.Model.Events.MealSets;

public class GetMealSetResponseLogger : FallibleEventLoggerAbstract<GetMealSetRequest, GetMealSetResponse>
{
    public GetMealSetResponseLogger(ILogger<GetMealSetResponseLogger> logger) : base(logger) { }

    protected override void OnSuccess(GetMealSetRequest request, GetMealSetResponse response)
    {
        Logger.LogInformation("Responded with {ResponseType}. MealSetId: {MealSetId}",
            nameof(GetMealSetResponse),
            response.Id);

        base.OnSuccess(request, response);
    }
}
