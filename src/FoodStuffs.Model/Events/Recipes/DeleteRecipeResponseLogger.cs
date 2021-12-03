using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

public class DeleteRecipeResponseLogger : EntityMessageEventLogger<DeleteRecipeRequest, int>
{
    public DeleteRecipeResponseLogger(ILogger<DeleteRecipeResponseLogger> logger) : base(logger) { }
}
