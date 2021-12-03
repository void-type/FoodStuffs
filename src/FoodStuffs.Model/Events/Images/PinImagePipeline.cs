using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class PinImagePipeline : EventPipelineAbstract<PinImageRequest, EntityMessage<int>>
{
    public PinImagePipeline(PinImageHandler handler, PinImageRequestLogger requestLogger, PinImageResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
