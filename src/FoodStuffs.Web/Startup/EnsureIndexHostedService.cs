using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Search;
using FoodStuffs.Model.Search.Recipes;
using FoodStuffs.Model.Search.Recipes.Models;

namespace FoodStuffs.Web.Startup;

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

        if (!Directory.Exists(config.GetIndexFolder(RecipeSearchConstants.INDEX_NAME)) || !Directory.Exists(config.GetTaxonomyFolder(RecipeSearchConstants.INDEX_NAME)))
        {
            _logger.LogWarning("Recipe index is missing on startup. Rebuilding.");
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
        }

        if (needsRebuild)
        {
            var index = scope.ServiceProvider.GetRequiredService<IRecipeIndexService>();
            await index.RebuildAsync(cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
