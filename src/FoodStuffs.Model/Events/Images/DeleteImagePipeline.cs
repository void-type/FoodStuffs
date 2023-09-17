using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class DeleteImagePipeline : EventPipelineAbstract<DeleteImageRequest, EntityMessage<string>>
{
    public DeleteImagePipeline(DeleteImageHandler handler, DeleteImageRequestLogger requestLogger, DeleteImageResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
