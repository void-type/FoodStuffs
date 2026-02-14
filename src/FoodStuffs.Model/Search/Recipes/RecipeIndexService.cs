using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Search.Lucene;
using Lucene.Net.Index;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VoidCore.EntityFramework;
using VoidCore.Model.Responses.Collections;
using C = FoodStuffs.Model.Search.Recipes.RecipeSearchConstants;

namespace FoodStuffs.Model.Search.Recipes;

public class RecipeIndexService : IRecipeIndexService
{
    private const int BATCH_SIZE = 100;

    private readonly ILogger<RecipeIndexService> _logger;
    private readonly SearchSettings _settings;
    private readonly FoodStuffsContext _data;
    private static readonly SemaphoreSlim _writeSemaphore = new(1, 1);

    public RecipeIndexService(ILogger<RecipeIndexService> logger, SearchSettings settings, FoodStuffsContext data)
    {
        _logger = logger;
        _settings = settings;
        _data = data;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(int recipeId, CancellationToken cancellationToken)
    {
        await UpdateAsync([recipeId], cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(IEnumerable<int> recipeId, CancellationToken cancellationToken)
    {
        var byId = new RecipesWithAllRelatedSpecification(recipeId);

        var recipes = await _data.Recipes
            .TagWith($"{nameof(RecipeIndexService)}.{nameof(UpdateAsync)}({nameof(RecipesWithAllRelatedSpecification)})")
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        if (recipes.Count == 0)
        {
            return;
        }

        await UpdateAsync(recipes, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(int recipeId, CancellationToken cancellationToken)
    {
        await RemoveAsync([recipeId], cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(IEnumerable<int> recipeIds, CancellationToken cancellationToken)
    {
        await _writeSemaphore.WaitAsync(cancellationToken);

        try
        {
            using var writers = new LuceneWriters(_settings, OpenMode.CREATE_OR_APPEND, C.INDEX_NAME);

            foreach (var recipeId in recipeIds)
            {
                writers.IndexWriter.DeleteDocuments(new Term(C.FIELD_ID, recipeId.ToString()));
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
        _logger.LogInformation("Starting rebuild of recipe search index.");

        await _writeSemaphore.WaitAsync(cancellationToken);

        try
        {
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
        finally
        {
            _writeSemaphore.Release();
        }
    }

    private async Task UpdateAsync(IEnumerable<Recipe> recipes, CancellationToken cancellationToken)
    {
        var recipesList = recipes.ToList();

        await _writeSemaphore.WaitAsync(cancellationToken);

        try
        {
            using var writers = new LuceneWriters(_settings, OpenMode.CREATE_OR_APPEND, C.INDEX_NAME);

            var facetsConfig = RecipeSearchHelper.FacetsConfig();

            foreach (var recipe in recipesList)
            {
                var doc = facetsConfig.Build(writers.TaxonomyWriter, recipe.ToDocument());
                writers.IndexWriter.UpdateDocument(new Term(C.FIELD_ID, recipe.Id.ToString()), doc);
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
