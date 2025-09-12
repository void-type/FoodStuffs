using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Search.Lucene;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using C = FoodStuffs.Model.Search.GroceryItems.GroceryItemSearchConstants;

namespace FoodStuffs.Model.Search.GroceryItems;

public class GroceryItemIndexService : IGroceryItemIndexService
{
    private const int BATCH_SIZE = 100;

    private readonly ILogger<GroceryItemIndexService> _logger;
    private readonly SearchSettings _settings;
    private readonly FoodStuffsContext _data;

    public GroceryItemIndexService(ILogger<GroceryItemIndexService> logger, SearchSettings settings, FoodStuffsContext data)
    {
        _logger = logger;
        _settings = settings;
        _data = data;
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateAsync(int groceryItemId, CancellationToken cancellationToken)
    {
        var byId = new GroceryItemsWithAllRelatedSpecification(groceryItemId);

        var maybeGroceryItem = await _data.GroceryItems
            .TagWith($"{nameof(GroceryItemIndexService)}.{nameof(AddOrUpdate)}({nameof(GroceryItemsWithAllRelatedSpecification)})")
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        if (maybeGroceryItem.HasNoValue)
        {
            return;
        }

        var groceryItem = maybeGroceryItem.Value;

        AddOrUpdate(groceryItem);
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateAsync(IEnumerable<int> groceryItemId, CancellationToken cancellationToken)
    {
        var byId = new GroceryItemsWithAllRelatedSpecification(groceryItemId);

        var groceryItems = await _data.GroceryItems
            .TagWith($"{nameof(GroceryItemIndexService)}.{nameof(AddOrUpdate)}({nameof(GroceryItemsWithAllRelatedSpecification)})")
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var groceryItem in groceryItems)
        {
            AddOrUpdate(groceryItem);
        }
    }

    /// <inheritdoc/>
    public void AddOrUpdate(GroceryItem groceryItem)
    {
        using var writers = new LuceneWriters(_settings, OpenMode.CREATE_OR_APPEND, C.INDEX_NAME);

        var facetsConfig = GroceryItemSearchHelper.FacetsConfig();

        var doc = facetsConfig.Build(writers.TaxonomyWriter, groceryItem.ToDocument());

        if (ExistsInIndex(groceryItem.Id))
        {
            writers.IndexWriter.UpdateDocument(new Term(C.FIELD_ID, groceryItem.Id.ToString()), doc);
        }
        else
        {
            writers.IndexWriter.AddDocument(doc);
        }

        writers.IndexWriter.Commit();
        writers.TaxonomyWriter.Commit();
    }

    /// <inheritdoc/>
    public async Task RebuildAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting rebuild of grocery item search index.");

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

    /// <inheritdoc/>
    public void Remove(int groceryItemId)
    {
        using var writers = new LuceneWriters(_settings, OpenMode.CREATE_OR_APPEND, C.INDEX_NAME);

        writers.IndexWriter.DeleteDocuments(new Term(C.FIELD_ID, groceryItemId.ToString()));

        writers.IndexWriter.Commit();
    }

    private bool ExistsInIndex(int groceryItemId)
    {
        using var readers = new LuceneReaders(_settings, C.INDEX_NAME);

        var query = new TermQuery(new Term(C.FIELD_ID, groceryItemId.ToString()));
        var topDocs = readers.IndexSearcher.Search(query, 1);

        return topDocs.TotalHits > 0;
    }
}
