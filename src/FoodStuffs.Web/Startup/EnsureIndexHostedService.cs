
using FoodStuffs.Model.Search;

namespace FoodStuffs.Web.Startup;

/// <summary>
/// This service will rebuild the index on startup if it doesn't exit.
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
        var config = scope.ServiceProvider.GetRequiredService<RecipeSearchSettings>();

        if (!Directory.Exists(config.IndexFolder) || !Directory.Exists(config.TaxonomyFolder))
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
