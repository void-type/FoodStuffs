using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

public class SaveRecipeResponseLogger : EntityMessageEventLogger<SaveRecipeRequest, int>
{
    public SaveRecipeResponseLogger(ILogger<SaveRecipeResponseLogger> logger) : base(logger) { }
}
