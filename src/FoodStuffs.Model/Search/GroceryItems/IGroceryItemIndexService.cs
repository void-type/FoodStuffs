namespace FoodStuffs.Model.Search.GroceryItems;

public interface IGroceryItemIndexService
{
    /// <summary>
    /// Query and update grocery item in the index.
    /// </summary>
    Task UpdateAsync(int groceryItemId, CancellationToken cancellationToken);

    /// <summary>
    /// Query and update multiple grocery items in the index.
    /// </summary>
    Task UpdateAsync(IEnumerable<int> groceryItemId, CancellationToken cancellationToken);

    /// <summary>
    /// Remove grocery item from the index.
    /// </summary>
    Task RemoveAsync(int groceryItemId, CancellationToken cancellationToken);

    /// <summary>
    /// Remove multiple grocery items from the index.
    /// </summary>
    Task RemoveAsync(IEnumerable<int> groceryItemIds, CancellationToken cancellationToken);

    /// <summary>
    /// Rebuild the index.
    /// </summary>
    Task RebuildAsync(CancellationToken cancellationToken);
}
