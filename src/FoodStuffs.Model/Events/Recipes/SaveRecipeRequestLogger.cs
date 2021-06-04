using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.Recipes
{
    public class SaveRecipeRequestLogger : RequestLoggerAbstract<SaveRecipeRequest>
    {
        public SaveRecipeRequestLogger(ILogger<SaveRecipeRequestLogger> logger) : base(logger) { }

        public override void Log(SaveRecipeRequest request)
        {
            Logger.LogInformation("Requested. RecipeId: {RecipeId}",
                request.Id);
        }
    }
}
