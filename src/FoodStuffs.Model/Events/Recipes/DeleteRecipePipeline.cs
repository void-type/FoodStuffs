using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes
{
    public class DeleteRecipePipeline : EventPipelineAbstract<DeleteRecipeRequest, EntityMessage<int>>
    {
        public DeleteRecipePipeline(DeleteRecipeHandler handler, DeleteRecipeRequestLogger requestLogger, DeleteRecipeResponseLogger responseLogger)
        {
            InnerHandler = handler
                .AddRequestLogger(requestLogger)
                .AddPostProcessor(responseLogger);
        }
    }
}
