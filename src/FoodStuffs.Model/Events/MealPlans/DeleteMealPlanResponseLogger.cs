using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealPlans;

public class DeleteMealPlanResponseLogger : EntityMessageEventLogger<DeleteMealPlanRequest, int>
{
    public DeleteMealPlanResponseLogger(ILogger<DeleteMealPlanResponseLogger> logger) : base(logger) { }
}
