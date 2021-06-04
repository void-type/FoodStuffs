using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images
{
    public class SaveImageResponseLogger : EntityMessageEventLogger<SaveImageRequest, int>
    {
        public SaveImageResponseLogger(ILogger<SaveImageResponseLogger> logger) : base(logger) { }
    }
}
