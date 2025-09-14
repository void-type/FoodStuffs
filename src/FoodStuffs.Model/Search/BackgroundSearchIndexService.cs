using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace FoodStuffs.Model.Search;

public class BackgroundSearchIndexService : BackgroundService, ISearchIndexService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BackgroundSearchIndexService> _logger;
    private readonly Channel<IIndexUpdateRequest> _queue;

    public BackgroundSearchIndexService(IServiceProvider serviceProvider, ILogger<BackgroundSearchIndexService> logger)
    {
        _queue = NewQueue();
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task AddOrUpdateAsync(SearchIndex indexName, int entityId, CancellationToken cancellationToken)
    {
        await AddOrUpdateAsync(indexName, [entityId], cancellationToken);
    }

    public async Task AddOrUpdateAsync(SearchIndex indexName, IEnumerable<int> entityIds, CancellationToken cancellationToken)
    {
        switch (indexName)
        {
            case SearchIndex.Recipes:
                await _queue.Writer.WriteAsync(new IndexUpdateRequest<IRecipeIndexService>
                {
                    Operation = indexService => indexService.AddOrUpdateAsync(entityIds, cancellationToken)
                }, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await _queue.Writer.WriteAsync(new IndexUpdateRequest<IGroceryItemIndexService>
                {
                    Operation = indexService => indexService.AddOrUpdateAsync(entityIds, cancellationToken)
                }, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(indexName), $"Unsupported index: {indexName}");
        }
    }

    public async Task RebuildAsync(SearchIndex indexName, CancellationToken cancellationToken)
    {
        switch (indexName)
        {
            case SearchIndex.Recipes:
                await _queue.Writer.WriteAsync(new IndexUpdateRequest<IRecipeIndexService>
                {
                    Operation = indexService => indexService.RebuildAsync(cancellationToken)
                }, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await _queue.Writer.WriteAsync(new IndexUpdateRequest<IGroceryItemIndexService>
                {
                    Operation = indexService => indexService.RebuildAsync(cancellationToken)
                }, cancellationToken);
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
                await _queue.Writer.WriteAsync(new IndexUpdateRequest<IRecipeIndexService>
                {
                    Operation = indexService =>
                    {
                        indexService.Remove(entityId);
                        return Task.CompletedTask;
                    }
                }, cancellationToken);
                break;

            case SearchIndex.GroceryItems:
                await _queue.Writer.WriteAsync(new IndexUpdateRequest<IGroceryItemIndexService>
                {
                    Operation = indexService =>
                    {
                        indexService.Remove(entityId);
                        return Task.CompletedTask;
                    }
                }, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(indexName), $"Unsupported index: {indexName}");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BackgroundSearchIndexService is starting.");

        try
        {
            await foreach (var request in _queue.Reader.ReadAllAsync(stoppingToken))
            {
                if (request is null)
                {
                    continue;
                }

                await ProcessRequestAsync(request);
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when cancellation is requested
        }
        finally
        {
            _logger.LogInformation("BackgroundSearchIndexService is stopping.");
        }
    }

    private async Task ProcessRequestAsync(IIndexUpdateRequest request)
    {
        using var scope = _serviceProvider.CreateScope();

        try
        {
            switch (request)
            {
                case IndexUpdateRequest<IRecipeIndexService> recipeRequest:
                    if (recipeRequest.Operation is not null)
                    {
                        var recipeIndexService = scope.ServiceProvider.GetRequiredService<IRecipeIndexService>();
                        await recipeRequest.Operation(recipeIndexService);
                    }
                    break;

                case IndexUpdateRequest<IGroceryItemIndexService> groceryItemRequest:
                    if (groceryItemRequest.Operation is not null)
                    {
                        var groceryItemIndexService = scope.ServiceProvider.GetRequiredService<IGroceryItemIndexService>();
                        await groceryItemRequest.Operation(groceryItemIndexService);
                    }
                    break;

                default:
                    _logger.LogWarning("Unknown index update request type: {RequestType}", request.GetType().Name);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing index update request of type {RequestType}", request.GetType().Name);
        }
    }

    private static Channel<IIndexUpdateRequest> NewQueue()
    {
        var options = new BoundedChannelOptions(1000)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        };

        return Channel.CreateBounded<IIndexUpdateRequest>(options);
    }

    private interface IIndexUpdateRequest
    {
    }

    private sealed class IndexUpdateRequest<TIndexService> : IIndexUpdateRequest
    {
        public Func<TIndexService, Task>? Operation { get; init; }
    }
}
