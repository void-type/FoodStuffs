using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Search;

public interface IRecipeIndexService
{
    /// <summary>
    /// Query and update recipe in the index.
    /// </summary>
    Task AddOrUpdate(int recipeId, CancellationToken cancellationToken);

    /// <summary>
    /// Update recipe in the index. Ensure you have a fully-hydrated recipe.
    /// </summary>
    void AddOrUpdate(Recipe recipe);

    /// <summary>
    /// Rebuild the index.
    /// </summary>
    Task Rebuild(CancellationToken cancellationToken);

    /// <summary>
    /// Remove recipe from the index.
    /// </summary>
    void Remove(int recipeId);
}
