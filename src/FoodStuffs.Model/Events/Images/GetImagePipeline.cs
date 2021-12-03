using VoidCore.Model.Events;
using VoidCore.Model.Responses.Files;

namespace FoodStuffs.Model.Events.Images;

public class GetImagePipeline : EventPipelineAbstract<GetImageRequest, SimpleFile>
{
    public GetImagePipeline(GetImageHandler handler, GetImageRequestLogger requestLogger, GetImageResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
