using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.MealSets;

public class GetMealSetPipeline : EventPipelineAbstract<GetMealSetRequest, GetMealSetResponse>
{
    public GetMealSetPipeline(GetMealSetHandler handler, GetMealSetRequestLogger requestLogger, GetMealSetResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
