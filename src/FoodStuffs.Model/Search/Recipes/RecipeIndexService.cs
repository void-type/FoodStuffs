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
using C = FoodStuffs.Model.Search.Recipes.RecipeSearchConstants;

namespace FoodStuffs.Model.Search.Recipes;

public class RecipeIndexService : IRecipeIndexService
{
    private const int BATCH_SIZE = 100;

    private readonly ILogger<RecipeIndexService> _logger;
    private readonly SearchSettings _settings;
    private readonly FoodStuffsContext _data;

    public RecipeIndexService(ILogger<RecipeIndexService> logger, SearchSettings settings, FoodStuffsContext data)
    {
        _logger = logger;
        _settings = settings;
        _data = data;
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateAsync(int recipeId, CancellationToken cancellationToken)
    {
        var byId = new RecipesWithAllRelatedSpecification(recipeId);

        var maybeRecipe = await _data.Recipes
            .TagWith($"{nameof(RecipeIndexService)}.{nameof(AddOrUpdate)}({nameof(RecipesWithAllRelatedSpecification)})")
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        if (maybeRecipe.HasNoValue)
        {
            return;
        }

        var recipe = maybeRecipe.Value;

        AddOrUpdate(recipe);
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateAsync(IEnumerable<int> recipeId, CancellationToken cancellationToken)
    {
        var byId = new RecipesWithAllRelatedSpecification(recipeId);

        var recipes = await _data.Recipes
            .TagWith($"{nameof(RecipeIndexService)}.{nameof(AddOrUpdate)}({nameof(RecipesWithAllRelatedSpecification)})")
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var recipe in recipes)
        {
            AddOrUpdate(recipe);
        }
    }

    /// <inheritdoc/>
    public void AddOrUpdate(Recipe recipe)
    {
        using var writers = new LuceneWriters(_settings, OpenMode.CREATE_OR_APPEND, C.INDEX_NAME);

        var facetsConfig = RecipeSearchHelper.FacetsConfig();

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

    /// <inheritdoc/>
    public async Task RebuildAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting rebuild of recipe search index.");

        using var writers = new LuceneWriters(_settings, OpenMode.CREATE, C.INDEX_NAME);

        var facetsConfig = RecipeSearchHelper.FacetsConfig();

        var page = 1;
        var numIndexed = 0;
        var done = false;

        do
        {
            var pagination = new PaginationOptions(page, BATCH_SIZE);
            var withAllRelated = new RecipesWithAllRelatedSpecification();

            var recipes = await _data.Recipes
                .TagWith($"{nameof(RecipeIndexService)}.{nameof(RebuildAsync)}({nameof(RecipesWithAllRelatedSpecification)})")
                .AsSplitQuery()
                .ApplyEfSpecification(withAllRelated)
                .OrderBy(x => x.Id)
                .GetPage(pagination)
                .ToListAsync(cancellationToken);

            foreach (var recipe in recipes)
            {
                var builtDoc = facetsConfig.Build(writers.TaxonomyWriter, recipe.ToDocument());
                writers.IndexWriter.AddDocument(builtDoc);
                numIndexed++;
            }

            done = recipes.Count < 1;
            page++;
        } while (!done);

        writers.IndexWriter.Commit();
        writers.TaxonomyWriter.Commit();

        _logger.LogInformation("Finished rebuild of recipe search index. {DocCount} documents.", numIndexed);
    }

    /// <inheritdoc/>
    public void Remove(int recipeId)
    {
        using var writers = new LuceneWriters(_settings, OpenMode.CREATE_OR_APPEND, C.INDEX_NAME);

        writers.IndexWriter.DeleteDocuments(new Term(C.FIELD_ID, recipeId.ToString()));

        writers.IndexWriter.Commit();
    }

    private bool ExistsInIndex(int recipeId)
    {
        using var readers = new LuceneReaders(_settings, C.INDEX_NAME);

        var query = new TermQuery(new Term(C.FIELD_ID, recipeId.ToString()));
        var topDocs = readers.IndexSearcher.Search(query, 1);

        return topDocs.TotalHits > 0;
    }
}
