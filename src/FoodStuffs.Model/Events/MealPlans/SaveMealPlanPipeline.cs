using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealPlans;

public class SaveMealPlanPipeline : EventPipelineAbstract<SaveMealPlanRequest, EntityMessage<int>>
{
    public SaveMealPlanPipeline(SaveMealPlanHandler handler, SaveMealPlanRequestLogger requestLogger, SaveMealPlanRequestValidator validator, SaveMealPlanResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddRequestValidator(validator)
            .AddPostProcessor(responseLogger);
    }
}
