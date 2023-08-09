using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.MealSets;

public class ListMealSetsResponseLogger : ItemSetEventLogger<ListMealSetsRequest, ListMealSetsResponse>
{
    public ListMealSetsResponseLogger(ILogger<ListMealSetsResponseLogger> logger) : base(logger) { }
}
