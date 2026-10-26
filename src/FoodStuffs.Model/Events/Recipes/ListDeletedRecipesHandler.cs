using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Recipes.Models;
using FoodStuffs.Model.RecycleBin;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

/// <summary>
/// Lists the recipes in the recycle bin, most recently deleted first.
/// </summary>
public class ListDeletedRecipesHandler(FoodStuffsContext data, RecycleBinSettings settings) : CustomEventHandlerAbstract<ListDeletedRecipesRequest, ListDeletedRecipesResponse>
{
    public override async Task<IResult<ListDeletedRecipesResponse>> Handle(ListDeletedRecipesRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var recipes = await data.Recipes
            .OnlySoftDeleted()
            .TagWith(GetTag())
            .OrderByDescending(r => r.DeletedOn)
            .ThenBy(r => r.Id)
            .ToItemSet(paginationOptions, cancellationToken);

        return Ok(new ListDeletedRecipesResponse(
            RetentionDays: settings.IsAutoPurgeEnabled ? settings.RetentionDays : null,
            Recipes: recipes.Select(r => new ListDeletedRecipesResponseRecipe(
                Id: r.Id,
                Name: r.Name,
                DeletedOn: r.DeletedOn,
                DeletedBy: r.DeletedBy,
                PurgeOn: settings.IsAutoPurgeEnabled ? r.DeletedOn?.AddDays(settings.RetentionDays) : null))));
    }
}
