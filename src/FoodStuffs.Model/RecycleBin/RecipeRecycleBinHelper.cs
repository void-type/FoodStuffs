using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;

namespace FoodStuffs.Model.RecycleBin;

internal static class RecipeRecycleBinHelper
{
    private const int BATCH_SIZE = 100;

    /// <summary>
    /// Permanently deletes the matching recipes that are in the recycle bin. Recipes are loaded so the soft-delete interceptor can verify
    /// they were deleted first. Search index entries were already removed when the recipes were deleted.
    /// </summary>
    /// <param name="data">The database context</param>
    /// <param name="filter">Limits which recipes in the recycle bin are deleted</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The number of recipes deleted</returns>
    public static async Task<int> PermanentlyDeleteAsync(
        FoodStuffsContext data,
        Func<IQueryable<Recipe>, IQueryable<Recipe>> filter,
        CancellationToken cancellationToken)
    {
        var total = 0;

        while (true)
        {
            var batch = await filter(data.Recipes.OnlySoftDeleted())
                .OrderBy(r => r.Id)
                .Take(BATCH_SIZE)
                .ToListAsync(cancellationToken);

            if (batch.Count == 0)
            {
                return total;
            }

            data.Recipes.RemoveRange(batch);

            await data.SaveChangesAsync(cancellationToken);

            total += batch.Count;
        }
    }
}
