namespace FoodStuffs.Model.Search;

public interface ISearchIndexService
{
    /// <summary>
    /// Query and update entity in the index.
    /// </summary>
    Task UpdateAsync(SearchIndex indexName, int entityId, CancellationToken cancellationToken);

    /// <summary>
    /// Query and update multiple entities in the index.
    /// </summary>
    Task UpdateAsync(SearchIndex indexName, IEnumerable<int> entityIds, CancellationToken cancellationToken);

    /// <summary>
    /// Remove entity from the index.
    /// </summary>
    Task RemoveAsync(SearchIndex indexName, int entityId, CancellationToken cancellationToken);

    /// <summary>
    /// Remove multiple entities from the index.
    /// </summary>
    Task RemoveAsync(SearchIndex indexName, IEnumerable<int> entityIds, CancellationToken cancellationToken);

    /// <summary>
    /// Enqueue entity IDs for deferred background index updates.
    /// </summary>
    void EnqueueUpdate(SearchIndex indexName, IEnumerable<int> entityIds);

    /// <summary>
    /// Rebuild the index.
    /// </summary>
    Task RebuildAsync(SearchIndex indexName, CancellationToken cancellationToken);
}
