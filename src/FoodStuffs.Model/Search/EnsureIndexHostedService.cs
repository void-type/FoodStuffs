using FoodStuffs.Model.Events.GroceryItems;
using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.GroceryItems.Models;
using FoodStuffs.Model.Search.Recipes;
using FoodStuffs.Model.Search.Recipes.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FoodStuffs.Model.Search;

/// <summary>
/// This service will rebuild the index on startup if it doesn't exist or if it's corrupted.
/// </summary>
public class EnsureIndexHostedService : IHostedService
{
    private readonly ILogger<EnsureIndexHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EnsureIndexHostedService(ILogger<EnsureIndexHostedService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var needsRebuild = false;

        // Check if index directories exist
        var config = scope.ServiceProvider.GetRequiredService<SearchSettings>();

        var folders = new[]
        {
            config.GetIndexFolder(RecipeSearchConstants.INDEX_NAME),
            config.GetTaxonomyFolder(RecipeSearchConstants.INDEX_NAME),
            config.GetIndexFolder(GroceryItemSearchConstants.INDEX_NAME),
            config.GetTaxonomyFolder(GroceryItemSearchConstants.INDEX_NAME)
        };

        if (folders.Any(x => !Directory.Exists(x)))
        {
            _logger.LogWarning("One or more index folders are missing on startup. Rebuilding.");
            needsRebuild = true;
        }
        else
        {
            // Test the index by performing a simple search
            try
            {
                var testRequest = new SearchRecipesRequest(
                    SearchText: null,
                    CategoryIds: null,
                    MatchAllCategories: false,
                    IsForMealPlanning: null,
                    SortBy: null,
                    RandomSortSeed: null,
                    IsPagingEnabled: true,
                    Page: 1,
                    Take: 5);

                var searchHandler = scope.ServiceProvider.GetRequiredService<SearchRecipesHandler>();
                await searchHandler.Handle(testRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Recipe index test failed. Index may be corrupted. Rebuilding.");
                needsRebuild = true;
            }

            // Test the index by performing a simple search
            try
            {
                var testRequest = new SearchGroceryItemsRequest(
                    SearchText: null,
                    StorageLocationIds: null,
                    MatchAllStorageLocations: false,
                    GroceryAisleIds: null,
                    IsOutOfStock: null,
                    IsUnused: null,
                    SortBy: null,
                    RandomSortSeed: null,
                    IsPagingEnabled: true,
                    Page: 1,
                    Take: 5);

                var searchHandler = scope.ServiceProvider.GetRequiredService<SearchGroceryItemsHandler>();
                await searchHandler.Handle(testRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Grocery items index test failed. Index may be corrupted. Rebuilding.");
                needsRebuild = true;
            }
        }

        if (needsRebuild)
        {
            var searchIndex = scope.ServiceProvider.GetRequiredService<ISearchIndexService>();
            await searchIndex.RebuildAsync(SearchIndex.Recipes, cancellationToken);
            await searchIndex.RebuildAsync(SearchIndex.GroceryItems, cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
