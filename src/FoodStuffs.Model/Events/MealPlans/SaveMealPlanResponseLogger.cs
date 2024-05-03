using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealPlans;

public class SaveMealPlanResponseLogger : EntityMessageEventLogger<SaveMealPlanRequest, int>
{
    public SaveMealPlanResponseLogger(ILogger<SaveMealPlanResponseLogger> logger) : base(logger) { }
}
