using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.Images
{
    public class GetImageRequestLogger : RequestLoggerAbstract<GetImageRequest>
    {
        public GetImageRequestLogger(ILogger<GetImageRequestLogger> logger) : base(logger) { }

        public override void Log(GetImageRequest request)
        {
            Logger.LogInformation("Requested. ImageId: {ImageId}",
                request.Id);
        }
    }
}
