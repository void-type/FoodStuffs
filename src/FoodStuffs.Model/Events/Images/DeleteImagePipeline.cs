using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

// Allow single file events
#pragma warning disable CA1034

namespace FoodStuffs.Model.Events.Images
{
    public class DeleteImagePipeline : EventPipelineAbstract<DeleteImageRequest, EntityMessage<int>>
    {
        public DeleteImagePipeline(DeleteImageHandler handler, DeleteImageRequestLogger requestLogger, DeleteImageResponseLogger responseLogger)
        {
            InnerHandler = handler
                .AddRequestLogger(requestLogger)
                .AddPostProcessor(responseLogger);
        }
    }
}
