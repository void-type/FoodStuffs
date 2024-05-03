using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.MealPlans;

public class ListMealPlansResponseLogger : ItemSetEventLogger<ListMealPlansRequest, ListMealPlansResponse>
{
    public ListMealPlansResponseLogger(ILogger<ListMealPlansResponseLogger> logger) : base(logger) { }
}
