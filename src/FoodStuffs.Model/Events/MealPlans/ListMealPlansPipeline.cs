using VoidCore.Model.Events;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.MealPlans;

public class ListMealPlansPipeline : EventPipelineAbstract<ListMealPlansRequest, IItemSet<ListMealPlansResponse>>
{
    public ListMealPlansPipeline(ListMealPlansHandler handler, ListMealPlansRequestLogger requestLogger, ListMealPlansResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
