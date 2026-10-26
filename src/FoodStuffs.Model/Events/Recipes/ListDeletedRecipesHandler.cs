using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Recipes.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

/// <summary>
/// Lists the recipes in the recycle bin, most recently deleted first.
/// </summary>
public class ListDeletedRecipesHandler(FoodStuffsContext data) : CustomEventHandlerAbstract<ListDeletedRecipesRequest, IItemSet<ListDeletedRecipesResponse>>
{
    public override async Task<IResult<IItemSet<ListDeletedRecipesResponse>>> Handle(ListDeletedRecipesRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        return await data.Recipes
            .OnlySoftDeleted()
            .TagWith(GetTag())
            .OrderByDescending(r => r.DeletedOn)
            .ThenBy(r => r.Id)
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(r => new ListDeletedRecipesResponse(
                Id: r.Id,
                Name: r.Name,
                DeletedOn: r.DeletedOn,
                DeletedBy: r.DeletedBy))
            .MapAsync(Ok);
    }
}
