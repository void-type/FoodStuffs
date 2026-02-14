using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Search.Lucene;
using Lucene.Net.Index;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VoidCore.EntityFramework;
using VoidCore.Model.Responses.Collections;
using C = FoodStuffs.Model.Search.GroceryItems.GroceryItemSearchConstants;

namespace FoodStuffs.Model.Search.GroceryItems;

public class GroceryItemIndexService : IGroceryItemIndexService
{
    private const int BATCH_SIZE = 100;

    private readonly ILogger<GroceryItemIndexService> _logger;
    private readonly SearchSettings _settings;
    private readonly FoodStuffsContext _data;
    private static readonly SemaphoreSlim _writeSemaphore = new(1, 1);

    public GroceryItemIndexService(ILogger<GroceryItemIndexService> logger, SearchSettings settings, FoodStuffsContext data)
    {
        _logger = logger;
        _settings = settings;
        _data = data;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(int groceryItemId, CancellationToken cancellationToken)
    {
        await UpdateAsync([groceryItemId], cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(IEnumerable<int> groceryItemId, CancellationToken cancellationToken)
    {
        var byId = new GroceryItemsWithAllRelatedSpecification(groceryItemId);

        var groceryItems = await _data.GroceryItems
            .TagWith($"{nameof(GroceryItemIndexService)}.{nameof(UpdateAsync)}({nameof(GroceryItemsWithAllRelatedSpecification)})")
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        if (groceryItems.Count == 0)
        {
            return;
        }

        await UpdateAsync(groceryItems, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(int groceryItemId, CancellationToken cancellationToken)
    {
        await RemoveAsync([groceryItemId], cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(IEnumerable<int> groceryItemIds, CancellationToken cancellationToken)
    {
        await _writeSemaphore.WaitAsync(cancellationToken);

        try
        {
            using var writers = new LuceneWriters(_settings, OpenMode.CREATE_OR_APPEND, C.INDEX_NAME);

            foreach (var groceryItemId in groceryItemIds)
            {
                writers.IndexWriter.DeleteDocuments(new Term(C.FIELD_ID, groceryItemId.ToString()));
            }

            writers.IndexWriter.Commit();
        }
        finally
        {
            _writeSemaphore.Release();
        }
    }

    /// <inheritdoc/>
    public async Task RebuildAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting rebuild of grocery item search index.");

        await _writeSemaphore.WaitAsync(cancellationToken);

        try
        {
            using var writers = new LuceneWriters(_settings, OpenMode.CREATE, C.INDEX_NAME);

            var facetsConfig = GroceryItemSearchHelper.FacetsConfig();

            var page = 1;
            var numIndexed = 0;
            var done = false;

            do
            {
                var pagination = new PaginationOptions(page, BATCH_SIZE);
                var withAllRelated = new GroceryItemsWithAllRelatedSpecification();

                var groceryItems = await _data.GroceryItems
                    .TagWith($"{nameof(GroceryItemIndexService)}.{nameof(RebuildAsync)}({nameof(GroceryItemsWithAllRelatedSpecification)})")
                    .AsSplitQuery()
                    .ApplyEfSpecification(withAllRelated)
                    .OrderBy(x => x.Id)
                    .GetPage(pagination)
                    .ToListAsync(cancellationToken);

                foreach (var groceryItem in groceryItems)
                {
                    var builtDoc = facetsConfig.Build(writers.TaxonomyWriter, groceryItem.ToDocument());
                    writers.IndexWriter.AddDocument(builtDoc);
                    numIndexed++;
                }

                done = groceryItems.Count < 1;
                page++;
            } while (!done);

            writers.IndexWriter.Commit();
            writers.TaxonomyWriter.Commit();

            _logger.LogInformation("Finished rebuild of grocery item search index. {DocCount} documents.", numIndexed);
        }
        finally
        {
            _writeSemaphore.Release();
        }
    }

    private async Task UpdateAsync(IEnumerable<GroceryItem> groceryItems, CancellationToken cancellationToken)
    {
        var groceryItemsList = groceryItems.ToList();

        await _writeSemaphore.WaitAsync(cancellationToken);

        try
        {
            using var writers = new LuceneWriters(_settings, OpenMode.CREATE_OR_APPEND, C.INDEX_NAME);

            var facetsConfig = GroceryItemSearchHelper.FacetsConfig();

            foreach (var groceryItem in groceryItemsList)
            {
                var doc = facetsConfig.Build(writers.TaxonomyWriter, groceryItem.ToDocument());
                writers.IndexWriter.UpdateDocument(new Term(C.FIELD_ID, groceryItem.Id.ToString()), doc);
            }

            writers.IndexWriter.Commit();
            writers.TaxonomyWriter.Commit();
        }
        finally
        {
            _writeSemaphore.Release();
        }
    }
}
