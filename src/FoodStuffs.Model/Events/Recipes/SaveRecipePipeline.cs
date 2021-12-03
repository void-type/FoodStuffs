using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

public class SaveRecipePipeline : EventPipelineAbstract<SaveRecipeRequest, EntityMessage<int>>
{
    public SaveRecipePipeline(SaveRecipeHandler handler, SaveRecipeRequestLogger requestLogger, SaveRecipeRequestValidator validator, SaveRecipeResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddRequestValidator(validator)
            .AddPostProcessor(responseLogger);
    }
}
