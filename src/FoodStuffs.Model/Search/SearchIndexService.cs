using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.Recipes;

namespace FoodStuffs.Model.Search;

public class SearchIndexService : ISearchIndexService
{
    private readonly IRecipeIndexService _recipeIndexService;
    private readonly IGroceryItemIndexService _groceryItemIndexService;
    private readonly SearchIndexBackgroundQueue _queue;

    public SearchIndexService(
        IRecipeIndexService recipeIndexService,
        IGroceryItemIndexService groceryItemIndexService,
        SearchIndexBackgroundQueue queue)
    {
        _recipeIndexService = recipeIndexService;
        _groceryItemIndexService = groceryItemIndexService;
        _queue = queue;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(SearchIndex indexName, int entityId, CancellationToken cancellationToken)
    {
        switch (indexName)
        {
            case SearchIndex.Recipes:
                await _recipeIndexService.UpdateAsync(entityId, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await _groceryItemIndexService.UpdateAsync(entityId, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(indexName), $"Unsupported index: {indexName}");
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(SearchIndex indexName, IEnumerable<int> entityIds, CancellationToken cancellationToken)
    {
        switch (indexName)
        {
            case SearchIndex.Recipes:
                await _recipeIndexService.UpdateAsync(entityIds, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await _groceryItemIndexService.UpdateAsync(entityIds, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(indexName), $"Unsupported index: {indexName}");
        }
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(SearchIndex indexName, int entityId, CancellationToken cancellationToken)
    {
        switch (indexName)
        {
            case SearchIndex.Recipes:
                await _recipeIndexService.RemoveAsync(entityId, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await _groceryItemIndexService.RemoveAsync(entityId, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(indexName), $"Unsupported index: {indexName}");
        }
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(SearchIndex indexName, IEnumerable<int> entityIds, CancellationToken cancellationToken)
    {
        switch (indexName)
        {
            case SearchIndex.Recipes:
                await _recipeIndexService.RemoveAsync(entityIds, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await _groceryItemIndexService.RemoveAsync(entityIds, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(indexName), $"Unsupported index: {indexName}");
        }
    }

    /// <inheritdoc/>
    public void EnqueueUpdate(SearchIndex indexName, IEnumerable<int> entityIds)
    {
        var ids = entityIds.ToList();
        _queue.Enqueue(new SearchIndexBackgroundAction(indexName, ids));
    }

    /// <inheritdoc/>
    public async Task RebuildAsync(SearchIndex indexName, CancellationToken cancellationToken)
    {
        _queue.Clear(indexName);

        switch (indexName)
        {
            case SearchIndex.Recipes:
                await _recipeIndexService.RebuildAsync(cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await _groceryItemIndexService.RebuildAsync(cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(indexName), $"Unsupported index: {indexName}");
        }
    }
}
