using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Files;

namespace FoodStuffs.Model.Events.Images
{
    public class GetImageResponseLogger : SimpleFileEventLogger<GetImageRequest>
    {
        public GetImageResponseLogger(ILogger<GetImageResponseLogger> logger) : base(logger) { }
    }
}
