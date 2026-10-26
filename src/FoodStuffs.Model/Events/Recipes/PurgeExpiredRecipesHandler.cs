using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Recipes.Models;
using FoodStuffs.Model.RecycleBin;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

/// <summary>
/// Permanently deletes recipes that have been in the recycle bin longer than <see cref="RecycleBinSettings.RetentionDays"/>.
/// </summary>
public class PurgeExpiredRecipesHandler(FoodStuffsContext data, RecycleBinSettings settings, TimeProvider timeProvider) : CustomEventHandlerAbstract<PurgeExpiredRecipesRequest, UserMessage>
{
    public override async Task<IResult<UserMessage>> Handle(PurgeExpiredRecipesRequest request, CancellationToken cancellationToken = default)
    {
        if (!settings.IsAutoPurgeEnabled)
        {
            return Ok(new UserMessage("Automatic purging of the recycle bin is disabled."));
        }

        var cutoff = timeProvider.GetUtcNow().AddDays(-settings.RetentionDays);

        var count = await RecipeRecycleBinHelper.PermanentlyDeleteAsync(data, q => q.Where(r => r.DeletedOn < cutoff), cancellationToken);

        return Ok(new UserMessage($"Purged {count} {(count == 1 ? "recipe" : "recipes")} deleted more than {settings.RetentionDays} days ago."));
    }
}
