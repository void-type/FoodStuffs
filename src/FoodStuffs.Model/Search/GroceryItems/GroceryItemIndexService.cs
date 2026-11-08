using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Search.Lucene;
using Lucene.Net.Index;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Responses.Collections;
using C = FoodStuffs.Model.Search.GroceryItems.GroceryItemSearchConstants;

namespace FoodStuffs.Model.Search.GroceryItems;

public class GroceryItemIndexService(ILogger<GroceryItemIndexService> logger, SearchSettings settings, FoodStuffsContext data) : IGroceryItemIndexService
{
    private const int BATCH_SIZE = 100;
    private static readonly SemaphoreSlim _writeSemaphore = new(1, 1);

    /// <inheritdoc/>
    public async Task UpdateAsync(int groceryItemId, CancellationToken cancellationToken)
    {
        await UpdateAsync([groceryItemId], cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(IEnumerable<int> groceryItemId, CancellationToken cancellationToken)
    {
        var byId = new GroceryItemsWithAllRelatedSpecification(groceryItemId);

        var groceryItems = await data.GroceryItems
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
            using var writers = new LuceneWriters(settings, OpenMode.CREATE_OR_APPEND, C.IndexName);

            foreach (var groceryItemId in groceryItemIds)
            {
                writers.IndexWriter.DeleteDocuments(new Term(C.FieldId, groceryItemId.ToString(CultureInfo.InvariantCulture)));
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
        GroceryItemIndexLogMessages.RebuildStarted(logger);

        await _writeSemaphore.WaitAsync(cancellationToken);

        try
        {
            LuceneDirectoryHelper.PrepareForRebuild(settings, C.IndexName);

            using var writers = new LuceneWriters(settings, OpenMode.CREATE, C.IndexName);

            var facetsConfig = GroceryItemSearchHelper.FacetsConfig();

            var page = 1;
            var numIndexed = 0;
            var done = false;

            do
            {
                var pagination = new PaginationOptions(page, BATCH_SIZE);
                var withAllRelated = new GroceryItemsWithAllRelatedSpecification();

                var groceryItems = await data.GroceryItems
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

            GroceryItemIndexLogMessages.RebuildFinished(logger, numIndexed);
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
            using var writers = new LuceneWriters(settings, OpenMode.CREATE_OR_APPEND, C.IndexName);

            var facetsConfig = GroceryItemSearchHelper.FacetsConfig();

            foreach (var groceryItem in groceryItemsList)
            {
                var doc = facetsConfig.Build(writers.TaxonomyWriter, groceryItem.ToDocument());
                writers.IndexWriter.UpdateDocument(new Term(C.FieldId, groceryItem.Id.ToString(CultureInfo.InvariantCulture)), doc);
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
