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
public class EnsureIndexHostedService(ILogger<EnsureIndexHostedService> logger, IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var needsRebuild = false;

        // Check if index directories exist
        var config = scope.ServiceProvider.GetRequiredService<SearchSettings>();

        var folders = new[]
        {
            config.GetIndexFolder(RecipeSearchConstants.IndexName),
            config.GetTaxonomyFolder(RecipeSearchConstants.IndexName),
            config.GetIndexFolder(GroceryItemSearchConstants.IndexName),
            config.GetTaxonomyFolder(GroceryItemSearchConstants.IndexName)
        };

        if (folders.Any(x => !Directory.Exists(x)))
        {
            EnsureIndexLogMessages.IndexFoldersMissing(logger);
            needsRebuild = true;
        }
        else
        {
            var recipeIndexHealthy = await TestRecipeIndexAsync(scope.ServiceProvider, cancellationToken);
            var groceryItemIndexHealthy = await TestGroceryItemIndexAsync(scope.ServiceProvider, cancellationToken);

            needsRebuild = !recipeIndexHealthy || !groceryItemIndexHealthy;
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

    // Test the index by performing a simple search
    private async Task<bool> TestRecipeIndexAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        try
        {
            var testRequest = new SearchRecipesRequest(
                SearchText: null,
                CategoryIds: null,
                MatchAllCategories: false,
                GroceryItemIds: null,
                MatchAllGroceryItems: false,
                IsForMealPlanning: null,
                SortBy: null,
                RandomSortSeed: null,
                IsPagingEnabled: true,
                Page: 1,
                Take: 5);

            var searchHandler = services.GetRequiredService<SearchRecipesHandler>();
            await searchHandler.Handle(testRequest, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            EnsureIndexLogMessages.RecipeIndexTestFailed(logger, ex);
            return false;
        }
    }

    // Test the index by performing a simple search
    private async Task<bool> TestGroceryItemIndexAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        try
        {
            var testRequest = new SearchGroceryItemsRequest(
                SearchText: null,
                StorageLocationIds: null,
                MatchAllStorageLocations: false,
                GroceryAisleIds: null,
                GroceryStoreIds: null,
                MatchAllGroceryStores: false,
                IsOutOfStock: null,
                IsUnused: null,
                SortBy: null,
                RandomSortSeed: null,
                IsPagingEnabled: true,
                Page: 1,
                Take: 5);

            var searchHandler = services.GetRequiredService<SearchGroceryItemsHandler>();
            await searchHandler.Handle(testRequest, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            EnsureIndexLogMessages.GroceryItemIndexTestFailed(logger, ex);
            return false;
        }
    }
}
