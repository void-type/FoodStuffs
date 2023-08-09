using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealSets;

public class SaveMealSetPipeline : EventPipelineAbstract<SaveMealSetRequest, EntityMessage<int>>
{
    public SaveMealSetPipeline(SaveMealSetHandler handler, SaveMealSetRequestLogger requestLogger, SaveMealSetRequestValidator validator, SaveMealSetResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddRequestValidator(validator)
            .AddPostProcessor(responseLogger);
    }
}
