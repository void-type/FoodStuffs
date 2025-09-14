namespace FoodStuffs.Model.Search;

public interface ISearchIndexService
{
    /// <summary>
    /// Query and update entity in the index.
    /// </summary>
    Task AddOrUpdateAsync(SearchIndex indexName, int entityId, CancellationToken cancellationToken);

    /// <summary>
    /// Query and update multiple entities in the index.
    /// </summary>
    Task AddOrUpdateAsync(SearchIndex indexName, IEnumerable<int> entityIds, CancellationToken cancellationToken);

    /// <summary>
    /// Rebuild the index.
    /// </summary>
    Task RebuildAsync(SearchIndex indexName, CancellationToken cancellationToken);

    /// <summary>
    /// Remove recipe from the index.
    /// </summary>
    Task RemoveAsync(SearchIndex indexName, int entityId, CancellationToken cancellationToken);
}
