using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Collections;
using C = FoodStuffs.Model.Search.RecipeSearchConstants;

namespace FoodStuffs.Model.Search;

public class RecipeIndexService : IRecipeIndexService
{
    private const int BATCH_SIZE = 100;

    private readonly ILogger<RecipeIndexService> _logger;
    private readonly RecipeSearchSettings _settings;
    private readonly IFoodStuffsData _data;

    public RecipeIndexService(ILogger<RecipeIndexService> logger, RecipeSearchSettings settings, IFoodStuffsData data)
    {
        _logger = logger;
        _settings = settings;
        _data = data;
    }

    public async Task Rebuild()
    {
        _logger.LogInformation("Starting rebuild of recipe search index.");

        using var writers = new LuceneWriters(_settings, C.Version, OpenMode.CREATE);

        var facetsConfig = DocumentMappers.RecipeFacetsConfig();

        var pagination = new PaginationOptions(1, BATCH_SIZE);
        var numIndexed = 0;
        var done = false;

        do
        {
            var page = await _data.Recipes.ListPage(new RecipesSearchSpecification([], pagination), CancellationToken.None);

            var recipes = page
                .Items
                .ToArray();

            foreach (var recipe in recipes)
            {
                var builtDoc = facetsConfig.Build(writers.TaxonomyWriter, recipe.ToDocument());
                writers.IndexWriter.AddDocument(builtDoc);
                numIndexed++;
            }

            done = numIndexed >= page.TotalCount;
            pagination = new PaginationOptions(pagination.Page + 1, BATCH_SIZE);
        } while (!done);

        writers.IndexWriter.Commit();
        writers.TaxonomyWriter.Commit();

        _logger.LogInformation("Finished rebuild of recipe search index. {DocCount} documents.", numIndexed);
    }

    public async Task AddOrUpdate(int recipeId, CancellationToken cancellationToken)
    {
        using var writers = new LuceneWriters(_settings, C.Version, OpenMode.CREATE_OR_APPEND);
        // Ensure index
        writers.IndexWriter.Commit();
        writers.TaxonomyWriter.Commit();

        var facetsConfig = DocumentMappers.RecipeFacetsConfig();

        var byId = new RecipesByIdWithAllRelatedSpecification(recipeId);

        var maybeRecipe = await _data.Recipes.Get(byId, cancellationToken);

        if (maybeRecipe.HasNoValue)
        {
            return;
        }

        var recipe = maybeRecipe.Value;

        var doc = facetsConfig.Build(writers.TaxonomyWriter, recipe.ToDocument());

        if (ExistsInIndex(recipe.Id))
        {
            writers.IndexWriter.UpdateDocument(new Term(C.FIELD_ID, recipe.Id.ToString()), doc);
        }
        else
        {
            writers.IndexWriter.AddDocument(doc);
        }

        writers.IndexWriter.Commit();
        writers.TaxonomyWriter.Commit();
    }

    public void Remove(int recipeId)
    {
        using var writers = new LuceneWriters(_settings, C.Version, OpenMode.CREATE_OR_APPEND);
        // Ensure index
        writers.IndexWriter.Commit();
        writers.TaxonomyWriter.Commit();

        writers.IndexWriter.DeleteDocuments(new Term(C.FIELD_ID, recipeId.ToString()));

        writers.IndexWriter.Commit();
    }

    private bool ExistsInIndex(int recipeId)
    {
        using var readers = new LuceneReaders(_settings);

        var query = new TermQuery(new Term(C.FIELD_ID, recipeId.ToString()));
        var topDocs = readers.IndexSearcher.Search(query, 1);

        return topDocs.TotalHits > 0;
    }
}
