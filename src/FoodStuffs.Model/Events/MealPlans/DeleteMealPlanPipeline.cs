using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealPlans;

public class DeleteMealPlanPipeline : EventPipelineAbstract<DeleteMealPlanRequest, EntityMessage<int>>
{
    public DeleteMealPlanPipeline(DeleteMealPlanHandler handler, DeleteMealPlanRequestLogger requestLogger, DeleteMealPlanResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
