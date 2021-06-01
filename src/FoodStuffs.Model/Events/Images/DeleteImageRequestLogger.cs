using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

// Allow single file events
#pragma warning disable CA1034

namespace FoodStuffs.Model.Events.Images
{
    public class DeleteImageRequestLogger : RequestLoggerAbstract<DeleteImageRequest>
    {
        public DeleteImageRequestLogger(ILogger<DeleteImageRequestLogger> logger) : base(logger) { }

        public override void Log(DeleteImageRequest request)
        {
            Logger.LogInformation("Requested. ImageId: {ImageId}",
                request.Id);
        }
    }
}
