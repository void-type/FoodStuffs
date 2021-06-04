using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.Images
{
    public class SaveImageRequestLogger : RequestLoggerAbstract<SaveImageRequest>
    {
        public SaveImageRequestLogger(ILogger<SaveImageRequestLogger> logger) : base(logger) { }

        public override void Log(SaveImageRequest request)
        {
            Logger.LogInformation("Requested. RecipeId: {RecipeId} FileSize: {FileSize}",
                request.RecipeId,
                request.FileContent.Length);
        }
    }
}
