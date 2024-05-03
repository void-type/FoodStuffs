using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses;

namespace FoodStuffs.Model.Events.MealPlans;

public class GetMealPlanResponseLogger : FallibleEventLoggerAbstract<GetMealPlanRequest, GetMealPlanResponse>
{
    public GetMealPlanResponseLogger(ILogger<GetMealPlanResponseLogger> logger) : base(logger) { }

    protected override void OnSuccess(GetMealPlanRequest request, GetMealPlanResponse response)
    {
        Logger.LogInformation("Responded with {ResponseType}. MealPlanId: {MealPlanId}",
            nameof(GetMealPlanResponse),
            response.Id);

        base.OnSuccess(request, response);
    }
}
