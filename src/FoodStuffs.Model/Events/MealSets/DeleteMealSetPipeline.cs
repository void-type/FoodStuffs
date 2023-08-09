using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealSets;

public class DeleteMealSetPipeline : EventPipelineAbstract<DeleteMealSetRequest, EntityMessage<int>>
{
    public DeleteMealSetPipeline(DeleteMealSetHandler handler, DeleteMealSetRequestLogger requestLogger, DeleteMealSetResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
