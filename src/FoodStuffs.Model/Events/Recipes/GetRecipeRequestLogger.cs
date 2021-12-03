using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.Recipes;

public class GetRecipeRequestLogger : RequestLoggerAbstract<GetRecipeRequest>
{
    public GetRecipeRequestLogger(ILogger<GetRecipeRequestLogger> logger) : base(logger) { }

    public override void Log(GetRecipeRequest request)
    {
        Logger.LogInformation("Requested. RecipeId: {RecipeId}",
            request.Id);
    }
}
