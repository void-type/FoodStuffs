using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealSets;

public class SaveMealSetResponseLogger : EntityMessageEventLogger<SaveMealSetRequest, int>
{
    public SaveMealSetResponseLogger(ILogger<SaveMealSetResponseLogger> logger) : base(logger) { }
}
