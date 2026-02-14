using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FoodStuffs.Model.Search;

public class SearchIndexBackgroundService : BackgroundService
{
    private readonly SearchIndexBackgroundQueue _queue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SearchIndexBackgroundService> _logger;

    public SearchIndexBackgroundService(SearchIndexBackgroundQueue queue, IServiceProvider serviceProvider, ILogger<SearchIndexBackgroundService> logger)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var action in _queue.Reader.ReadAllAsync(stoppingToken))
        {
            // Grab this action plus any others already queued
            var pending = new List<SearchIndexBackgroundAction> { action };

            while (_queue.Reader.TryRead(out var additional))
            {
                pending.Add(additional);
            }

            // Merge by index name, deduplicate entity IDs
            var merged = pending
                .GroupBy(a => a.IndexName)
                .Select(g => new SearchIndexBackgroundAction(
                    g.Key,
                    g.SelectMany(a => a.EntityIds).Distinct().ToList()));

            foreach (var mergedAction in merged)
            {
                await ProcessActionAsync(mergedAction, stoppingToken);
                await Task.Yield(); // Give immediate requests a chance at the semaphore
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _queue.Complete();
        await base.StopAsync(cancellationToken);

        // ExecuteAsync is now stopped, safe to drain remaining items
        while (_queue.Reader.TryRead(out var action))
        {
            await ProcessActionAsync(action, cancellationToken);
        }
    }

    private async Task ProcessActionAsync(SearchIndexBackgroundAction action, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var searchIndex = scope.ServiceProvider.GetRequiredService<ISearchIndexService>();
            await searchIndex.UpdateAsync(action.IndexName, action.EntityIds, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing deferred search index action for {IndexName} with {Count} entities.",
                action.IndexName, action.EntityIds.Count);
        }
    }
}
