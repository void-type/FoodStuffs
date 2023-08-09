using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealSets;

public class DeleteMealSetResponseLogger : EntityMessageEventLogger<DeleteMealSetRequest, int>
{
    public DeleteMealSetResponseLogger(ILogger<DeleteMealSetResponseLogger> logger) : base(logger) { }
}
