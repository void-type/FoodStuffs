using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Search.Recipes;

public interface IRecipeIndexService
{
    /// <summary>
    /// Query and update recipe in the index.
    /// </summary>
    Task AddOrUpdateAsync(int recipeId, CancellationToken cancellationToken);

    /// <summary>
    /// Query and update multiple recipes in the index.
    /// </summary>
    Task AddOrUpdateAsync(IEnumerable<int> recipeId, CancellationToken cancellationToken);

    /// <summary>
    /// Update recipe in the index. Ensure you have a fully-hydrated recipe.
    /// </summary>
    void AddOrUpdate(Recipe recipe);

    /// <summary>
    /// Rebuild the index.
    /// </summary>
    Task RebuildAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Remove recipe from the index.
    /// </summary>
    void Remove(int recipeId);
}
