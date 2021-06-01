using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

// Allow single file events
#pragma warning disable CA1034

namespace FoodStuffs.Model.Events.Images
{
    public class DeleteImageResponseLogger : EntityMessageEventLogger<DeleteImageRequest, int>
    {
        public DeleteImageResponseLogger(ILogger<DeleteImageResponseLogger> logger) : base(logger) { }
    }
}
