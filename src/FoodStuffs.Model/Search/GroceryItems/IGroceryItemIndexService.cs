using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Search.GroceryItems;

public interface IGroceryItemIndexService
{
    /// <summary>
    /// Query and update grocery item in the index.
    /// </summary>
    Task AddOrUpdateAsync(int groceryItemId, CancellationToken cancellationToken);

    /// <summary>
    /// Query and update multiple grocery items in the index.
    /// </summary>
    Task AddOrUpdateAsync(IEnumerable<int> groceryItemId, CancellationToken cancellationToken);

    /// <summary>
    /// Update grocery item in the index. Ensure you have a fully-hydrated grocery item.
    /// </summary>
    void AddOrUpdate(GroceryItem groceryItem);

    /// <summary>
    /// Rebuild the index.
    /// </summary>
    Task RebuildAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Remove grocery item from the index.
    /// </summary>
    void Remove(int groceryItemId);
}
