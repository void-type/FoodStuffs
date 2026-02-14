namespace FoodStuffs.Model.Search.Recipes;

public interface IRecipeIndexService
{
    /// <summary>
    /// Query and update recipe in the index.
    /// </summary>
    Task UpdateAsync(int recipeId, CancellationToken cancellationToken);

    /// <summary>
    /// Query and update multiple recipes in the index.
    /// </summary>
    Task UpdateAsync(IEnumerable<int> recipeId, CancellationToken cancellationToken);

    /// <summary>
    /// Remove recipe from the index.
    /// </summary>
    Task RemoveAsync(int recipeId, CancellationToken cancellationToken);

    /// <summary>
    /// Remove multiple recipes from the index.
    /// </summary>
    Task RemoveAsync(IEnumerable<int> recipeIds, CancellationToken cancellationToken);

    /// <summary>
    /// Rebuild the index.
    /// </summary>
    Task RebuildAsync(CancellationToken cancellationToken);
}
