using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.Recipes;

namespace FoodStuffs.Model.Search;

public class SearchIndexService : ISearchIndexService
{
    private readonly IRecipeIndexService _recipeIndexService;
    private readonly IGroceryItemIndexService _groceryItemIndexService;

    public SearchIndexService(
        IRecipeIndexService recipeIndexService,
        IGroceryItemIndexService groceryItemIndexService)
    {
        _recipeIndexService = recipeIndexService;
        _groceryItemIndexService = groceryItemIndexService;
    }

    public async Task AddOrUpdateAsync(SearchIndex indexName, int entityId, CancellationToken cancellationToken)
    {
        switch (indexName)
        {
            case SearchIndex.Recipes:
                await _recipeIndexService.AddOrUpdateAsync(entityId, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await _groceryItemIndexService.AddOrUpdateAsync(entityId, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(indexName), $"Unsupported index: {indexName}");
        }
    }

    public async Task AddOrUpdateAsync(SearchIndex indexName, IEnumerable<int> entityIds, CancellationToken cancellationToken)
    {
        switch (indexName)
        {
            case SearchIndex.Recipes:
                await _recipeIndexService.AddOrUpdateAsync(entityIds, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await _groceryItemIndexService.AddOrUpdateAsync(entityIds, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(indexName), $"Unsupported index: {indexName}");
        }
    }

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

        await Task.CompletedTask;
    }

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

        await Task.CompletedTask;
    }

    public async Task RebuildAsync(SearchIndex indexName, CancellationToken cancellationToken)
    {
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
