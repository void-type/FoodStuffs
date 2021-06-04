using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.Images
{
    public class PinImageRequestLogger : RequestLoggerAbstract<PinImageRequest>
    {
        public PinImageRequestLogger(ILogger<PinImageRequestLogger> logger) : base(logger) { }

        public override void Log(PinImageRequest request)
        {
            Logger.LogInformation("Requested. ImageId: {ImageId}",
                request.Id);
        }
    }
}
