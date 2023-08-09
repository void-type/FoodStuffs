using VoidCore.Model.Events;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.MealSets;

public class ListMealSetsPipeline : EventPipelineAbstract<ListMealSetsRequest, IItemSet<ListMealSetsResponse>>
{
    public ListMealSetsPipeline(ListMealSetsHandler handler, ListMealSetsRequestLogger requestLogger, ListMealSetsResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
