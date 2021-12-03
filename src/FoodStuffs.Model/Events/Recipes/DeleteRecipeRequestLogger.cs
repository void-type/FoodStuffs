using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.Recipes;

public class DeleteRecipeRequestLogger : RequestLoggerAbstract<DeleteRecipeRequest>
{
    public DeleteRecipeRequestLogger(ILogger<DeleteRecipeRequestLogger> logger) : base(logger) { }

    public override void Log(DeleteRecipeRequest request)
    {
        Logger.LogInformation("Requested. RecipeId: {RecipeId}",
            request.Id);
    }
}
