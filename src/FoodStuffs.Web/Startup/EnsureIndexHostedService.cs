
using FoodStuffs.Model.Search;
using FoodStuffs.Model.Search.Recipes;

namespace FoodStuffs.Web.Startup;

/// <summary>
/// This service will rebuild the index on startup if it doesn't exist.
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

        var index = scope.ServiceProvider.GetRequiredService<IRecipeIndexService>();
        var config = scope.ServiceProvider.GetRequiredService<SearchSettings>();

        if (!Directory.Exists(config.GetIndexFolder(RecipeSearchConstants.INDEX_NAME)) || !Directory.Exists(config.GetTaxonomyFolder(RecipeSearchConstants.INDEX_NAME)))
        {
            _logger.LogWarning("Recipe index is missing on startup. Rebuilding.");
            await index.Rebuild(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
