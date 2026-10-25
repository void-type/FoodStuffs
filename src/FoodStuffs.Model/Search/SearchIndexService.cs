using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.Recipes;

namespace FoodStuffs.Model.Search;

public class SearchIndexService(
    IRecipeIndexService recipeIndexService,
    IGroceryItemIndexService groceryItemIndexService,
    SearchIndexBackgroundQueue queue) : ISearchIndexService
{

    /// <inheritdoc/>
    public async Task UpdateAsync(SearchIndex indexName, int entityId, CancellationToken cancellationToken)
    {
        switch (indexName)
        {
            case SearchIndex.Recipes:
                await recipeIndexService.UpdateAsync(entityId, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await groceryItemIndexService.UpdateAsync(entityId, cancellationToken);
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
                await recipeIndexService.UpdateAsync(entityIds, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await groceryItemIndexService.UpdateAsync(entityIds, cancellationToken);
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
                await recipeIndexService.RemoveAsync(entityId, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await groceryItemIndexService.RemoveAsync(entityId, cancellationToken);
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
                await recipeIndexService.RemoveAsync(entityIds, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await groceryItemIndexService.RemoveAsync(entityIds, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(indexName), $"Unsupported index: {indexName}");
        }
    }

    /// <inheritdoc/>
    public void EnqueueUpdate(SearchIndex indexName, IEnumerable<int> entityIds)
    {
        var ids = entityIds.ToList();
        queue.Enqueue(new SearchIndexBackgroundAction(indexName, ids));
    }

    /// <inheritdoc/>
    public async Task RebuildAsync(SearchIndex indexName, CancellationToken cancellationToken)
    {
        queue.Clear(indexName);

        switch (indexName)
        {
            case SearchIndex.Recipes:
                await recipeIndexService.RebuildAsync(cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await groceryItemIndexService.RebuildAsync(cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(indexName), $"Unsupported index: {indexName}");
        }
    }
}
