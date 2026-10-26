using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Events.Recipes.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FoodStuffs.Model.RecycleBin;

/// <summary>
/// Once a day, permanently deletes recipes that have been in the recycle bin longer than the configured retention.
/// </summary>
public class RecycleBinPurgeHostedService(IServiceProvider serviceProvider, ILogger<RecycleBinPurgeHostedService> logger) : BackgroundService
{
    private static readonly TimeSpan _startupDelay = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan _interval = TimeSpan.FromDays(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Stay out of the way of startup work like migrations and search indexing.
        await Task.Delay(_startupDelay, stoppingToken);

        using var timer = new PeriodicTimer(_interval);

        do
        {
            await PurgeAsync(stoppingToken);
        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async Task PurgeAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();

            var result = await scope.ServiceProvider
                .GetRequiredService<PurgeExpiredRecipesHandler>()
                .Handle(new PurgeExpiredRecipesRequest(), cancellationToken);

            logger.LogInformation("{Message}", result.Value.Message);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // An unhandled exception would stop the host, and the purge will try again tomorrow.
            logger.LogError(ex, "Error purging the recycle bin.");
        }
    }
}
