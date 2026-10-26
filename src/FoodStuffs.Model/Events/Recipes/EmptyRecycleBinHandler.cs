using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Recipes.Models;
using FoodStuffs.Model.RecycleBin;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

/// <summary>
/// Permanently deletes every recipe in the recycle bin.
/// </summary>
public class EmptyRecycleBinHandler(FoodStuffsContext data) : CustomEventHandlerAbstract<EmptyRecycleBinRequest, UserMessage>
{
    public override async Task<IResult<UserMessage>> Handle(EmptyRecycleBinRequest request, CancellationToken cancellationToken = default)
    {
        var count = await RecipeRecycleBinHelper.PermanentlyDeleteAsync(data, q => q, cancellationToken);

        return Ok(new UserMessage($"Recycle bin emptied. {count} {(count == 1 ? "recipe" : "recipes")} permanently deleted."));
    }
}
