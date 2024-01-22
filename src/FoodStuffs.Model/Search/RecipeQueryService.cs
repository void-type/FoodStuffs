using FoodStuffs.Model.Events.Recipes;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Facet;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Time;
using C = FoodStuffs.Model.Search.RecipeSearchConstants;

namespace FoodStuffs.Model.Search;

public class RecipeQueryService : IRecipeQueryService
{
    private readonly IDateTimeService _dateTimeService;
    private readonly RecipeSearchSettings _settings;

    public RecipeQueryService(RecipeSearchSettings settings, IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
        _settings = settings;
    }

    public IItemSet<SearchRecipesResponse> Search(SearchRecipesRequest request)
    {
        using (var writers = new LuceneWriters(_settings, C.Version, OpenMode.CREATE_OR_APPEND))
        {
            // Ensure index
            writers.IndexWriter.Commit();
            writers.TaxonomyWriter.Commit();
        }

        using var readers = new LuceneReaders(_settings);
        var searcher = readers.IndexSearcher;

        var facetsConfig = DocumentMappers.RecipeFacetsConfig();

        var query = new BooleanQuery()
        {
            { BuildTextQuery(request), Occur.MUST },
            { BuildFilterQuery(request, facetsConfig), Occur.MUST },
        };

        var sort = BuildSortCriteria(request);

        var topDocs = sort is null
            ? searcher.Search(query, C.MAX_RESULTS)
            : searcher.Search(query, C.MAX_RESULTS, sort);

        var pagination = request.GetPaginationOptions();

        var scoreDocs = new List<ScoreDoc>(topDocs.ScoreDocs);

        if (request.SortBy?.ToUpperInvariant() == "RANDOM")
        {
            // Perform a random sort on the whole search set.
            // Sort is stable within same local day.
            var random = new Random(_dateTimeService.MomentWithOffset.Date.GetHashCode());

            // Ignore weak random
#pragma warning disable SCS0005
            scoreDocs.Sort((_, __) => random.Next(-1, 2));
#pragma warning restore SCS0005
        }

        return scoreDocs
            .GetPage(pagination)
            .Select(x => searcher.Doc(x.Doc).ToSearchRecipesResponse())
            .ToItemSet(pagination, topDocs.TotalHits);
    }

    private static Query BuildTextQuery(SearchRecipesRequest request)
    {
        var analyzer = new StandardAnalyzer(C.Version);
        var queryBuilder = new QueryBuilder(analyzer);
        var query = new BooleanQuery();

        // Exclude small queries that break the query builders.
        if (!string.IsNullOrWhiteSpace(request.NameSearch) && request.NameSearch.Length > 1)
        {
            var searchText = request.NameSearch.Trim().ToLowerInvariant();

            // If the exact phrase is found (with the given slop), it will be boosted by the given amount.
            var phraseQuery = queryBuilder.CreatePhraseQuery(C.FIELD_NAME, searchText, 2);

            // CreatePhraseQuery can return null in some cases.
            phraseQuery.Boost = 9;
            query.Add(phraseQuery, Occur.SHOULD);

            // Also add searching for ANY (SHOULD) of the given terms. If at least one term is found in the field, it will match but be scored lower than phrase.
            // If you would like ALL terms to match, use MUST instead of SHOULD.
            var wordQuery = queryBuilder.CreateBooleanQuery(C.FIELD_NAME, searchText, Occur.SHOULD);
            wordQuery.Boost = 3;
            query.Add(wordQuery, Occur.SHOULD);

            // Append a wildcard to the last term.
            var lastTerm = searchText.Split(" ").LastOrDefault();

            if (!string.IsNullOrWhiteSpace(lastTerm))
            {
                var lastWildcard = new WildcardQuery(new Term(C.FIELD_NAME, lastTerm + "*"));
                lastWildcard.Boost = 1;
                query.Add(lastWildcard, Occur.SHOULD);
            }

            // Partial word matches using fuzzy queries
            var fuzzyQuery = new FuzzyQuery(new Term(C.FIELD_NAME, searchText), 2);
            fuzzyQuery.Boost = 0.3f;
            query.Add(fuzzyQuery, Occur.SHOULD);
        }

        if (query.Clauses.Count < 1)
        {
            return new MatchAllDocsQuery();
        }

        return query;
    }

    private static Query BuildFilterQuery(SearchRecipesRequest request, FacetsConfig facetsConfig)
    {
        var query = new BooleanQuery();

        if (request.CategoryIds?.Length > 0)
        {
            var categoriesQuery = new BooleanQuery();

            foreach (var categoryId in request.CategoryIds)
            {
                var drillDownQuery = new DrillDownQuery(facetsConfig)
                {
                    { C.FIELD_CATEGORY_IDS, categoryId.ToString() }
                };

                categoriesQuery.Add(drillDownQuery, Occur.SHOULD);
            }

            query.Add(categoriesQuery, Occur.MUST);
        }

        if (request.IsForMealPlanning is not null)
        {
            var drillDownQuery = new DrillDownQuery(facetsConfig)
            {
                { C.FIELD_IS_FOR_MEAL_PLANNING, request.IsForMealPlanning.ToString() }
            };

            query.Add(drillDownQuery, Occur.MUST);
        }

        if (query.Clauses.Count < 1)
        {
            return new MatchAllDocsQuery();
        }

        return query;
    }

    private static Sort? BuildSortCriteria(SearchRecipesRequest request)
    {
        // If "RANDOM", we shuffle topDocs results later.
        return (request.SortBy?.ToUpperInvariant()) switch
        {
            "NEWEST" => new Sort(
                new SortField(C.FIELD_CREATED_ON, SortFieldType.STRING, true)),
            "OLDEST" => new Sort(
                new SortField(C.FIELD_CREATED_ON, SortFieldType.STRING, false)),
            "A-Z" => new Sort(
                new SortField(C.FIELD_NAME, SortFieldType.STRING, false),
                new SortField(C.FIELD_CREATED_ON, SortFieldType.STRING, false)),
            "Z-A" => new Sort(
                new SortField(C.FIELD_NAME, SortFieldType.STRING, true),
                new SortField(C.FIELD_CREATED_ON, SortFieldType.STRING, true)),
            _ => null,
        };
    }
}
