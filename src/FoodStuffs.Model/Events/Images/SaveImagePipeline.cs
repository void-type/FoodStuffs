using VoidCore.Model.Events;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images
{
    public class SaveImagePipeline : EventPipelineAbstract<SaveImageRequest, EntityMessage<int>>
    {
        public SaveImagePipeline(SaveImageHandler handler, SaveImageRequestLogger requestLogger, SaveImageResponseLogger responseLogger)
        {
            InnerHandler = handler
                .AddRequestLogger(requestLogger)
                .AddPostProcessor(responseLogger);
        }
    }
}
