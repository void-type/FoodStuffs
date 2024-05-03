using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.MealPlans;

public class GetMealPlanPipeline : EventPipelineAbstract<GetMealPlanRequest, GetMealPlanResponse>
{
    public GetMealPlanPipeline(GetMealPlanHandler handler, GetMealPlanRequestLogger requestLogger, GetMealPlanResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
