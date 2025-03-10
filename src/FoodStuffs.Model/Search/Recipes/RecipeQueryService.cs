using FoodStuffs.Model.Search.Recipes.Models;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Facet;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Time;
using C = FoodStuffs.Model.Search.Recipes.RecipeSearchConstants;

namespace FoodStuffs.Model.Search.Recipes;

public class RecipeQueryService : IRecipeQueryService
{
    private readonly IDateTimeService _dateTimeService;
    private readonly SearchSettings _settings;

    public RecipeQueryService(SearchSettings settings, IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
        _settings = settings;
    }

    /// <inheritdoc/>
    public SearchRecipesResponse Search(SearchRecipesRequest request)
    {
        using var readers = new LuceneReaders(_settings, C.INDEX_NAME);
        var searcher = readers.IndexSearcher;
        var facetsCollector = new FacetsCollector();

        var facetsConfig = RecipeSearchHelper.RecipeFacetsConfig();

        var query = new BooleanQuery()
        {
            { BuildSearchTextQuery(request.SearchText), Occur.MUST },
        };

        var filter = BuildFilter(request, facetsConfig);
        var sort = BuildSortCriteria(request);

        var topDocs = sort is null
            ? FacetsCollector.Search(searcher, query, filter, _settings.MaxResults, facetsCollector)
            : FacetsCollector.Search(searcher, query, filter, _settings.MaxResults, sort, facetsCollector);

        var pagination = request.GetPaginationOptions();

        var scoreDocs = new List<ScoreDoc>(topDocs.ScoreDocs);

        if (request.SortBy?.ToUpperInvariant() == "RANDOM")
        {
            var seed = request.RandomSortSeed is not null ?
                // Let the client give us a seed.
                request.RandomSortSeed.GetHashCode() :
                // Sort is stable within same local day.
                _dateTimeService.MomentWithOffset.Date.GetHashCode();

            var random = new Random(seed);

            // Ignore weak random
#pragma warning disable SCS0005
            // Perform a random sort on the whole search set.
            scoreDocs.Sort((_, __) => random.Next(-1, 2));
#pragma warning restore SCS0005
        }

        var resultItems = scoreDocs
            .GetPage(pagination)
            .Select(x => searcher.Doc(x.Doc).ToSearchRecipesResultItem())
            .ToItemSet(pagination, topDocs.TotalHits);

        var resultFacets = SearchHelper.GetFacets(readers, facetsCollector, facetsConfig);

        return new(resultItems, resultFacets);
    }

    public IItemSet<SuggestRecipesResultItem> Suggest(SuggestRecipesRequest request)
    {
        // In the future we should use Lucene suggester.
        try
        {
            if (string.IsNullOrWhiteSpace(request.SearchText) || request.SearchText.Length <= 1)
            {
                return new List<SuggestRecipesResultItem>().ToItemSet(request.GetPaginationOptions());
            }

            using var readers = new LuceneReaders(_settings, C.INDEX_NAME);
            var searcher = readers.IndexSearcher;

            var query = new BooleanQuery()
            {
                { BuildSuggestTextQuery(request.SearchText), Occur.MUST },
            };

            var pagination = request.GetPaginationOptions();

            var topDocs = searcher.Search(query, request.Take);

            return topDocs.ScoreDocs
                .GetPage(pagination)
                .Select(x => searcher.Doc(x.Doc).ToSuggestRecipesResultItem())
                .ToItemSet(pagination, topDocs.TotalHits);
        }
        catch
        {
            return new List<SuggestRecipesResultItem>().ToItemSet(request.GetPaginationOptions());
        }
    }

    private Query BuildSearchTextQuery(string? requestText)
    {
        var searchText = requestText?.Trim()?.ToLowerInvariant();

        var analyzer = new StandardAnalyzer(_settings.LuceneVersion);
        var queryBuilder = new QueryBuilder(analyzer);
        var query = new BooleanQuery();

        // Exclude small queries that break the query builders.
        if (!string.IsNullOrWhiteSpace(searchText) && searchText.Length > 1)
        {
            // If the exact phrase is found (with the given word slop), it will be boosted by the given amount.
            // Word slop is the number of intervening words allowed between the terms in the phrase.
            // CreatePhraseQuery can return null in some cases.
            var phraseQuery = queryBuilder.CreatePhraseQuery(C.FIELD_NAME, searchText, 2);
            phraseQuery.Boost = 9;
            query.Add(phraseQuery, Occur.SHOULD);

            // Query for ANY (SHOULD) of the given terms. If at least one term is found in the field, it will match but be scored lower than phrase.
            // If you would like ALL terms to match, use MUST instead of SHOULD.
            var wordQuery = queryBuilder.CreateBooleanQuery(C.FIELD_NAME, searchText, Occur.SHOULD);
            wordQuery.Boost = 3;
            query.Add(wordQuery, Occur.SHOULD);

            // Append a wildcard to the last term for incomplete text matches.
            var lastTerm = searchText.Split(" ").LastOrDefault();

            if (!string.IsNullOrWhiteSpace(lastTerm))
            {
                var lastWildcard = new WildcardQuery(new Term(C.FIELD_NAME, lastTerm + "*"))
                {
                    Boost = 3
                };

                query.Add(lastWildcard, Occur.SHOULD);
            }

            // Partial word matches using fuzzy queries to account for spelling errors and similar words.
            // The Levenshtein distance is the number of single-character edits (insertions, deletions, or substitutions) required to change one word into the other.
            var fuzzyQuery = new FuzzyQuery(new Term(C.FIELD_NAME, searchText), 2)
            {
                Boost = 0.3f
            };

            query.Add(fuzzyQuery, Occur.SHOULD);
        }

        if (query.Clauses.Count < 1)
        {
            return new MatchAllDocsQuery();
        }

        return query;
    }

    private Query BuildSuggestTextQuery(string? requestText)
    {
        var searchText = requestText?.Trim()?.ToLowerInvariant();

        var analyzer = new StandardAnalyzer(_settings.LuceneVersion);
        var queryBuilder = new QueryBuilder(analyzer);
        var query = new BooleanQuery();

        // Exclude small queries that break the query builders.
        if (!string.IsNullOrWhiteSpace(searchText) && searchText.Length > 1)
        {
            // Boost when the exact phrase is at the beginning of the field.
            var prefixQuery = new PrefixQuery(new Term(C.FIELD_NAME_PREFIX, searchText.ToLower()))
            {
                Boost = 18
            };
            query.Add(prefixQuery, Occur.SHOULD);

            // If the exact phrase is found (with the given word slop), it will be boosted by the given amount.
            // Word slop is the number of intervening words allowed between the terms in the phrase.
            // CreatePhraseQuery can return null in some cases.
            var phraseQuery = queryBuilder.CreatePhraseQuery(C.FIELD_NAME, searchText, 2);
            phraseQuery.Boost = 9;
            query.Add(phraseQuery, Occur.SHOULD);

            // Query for ANY (SHOULD) of the given terms. If at least one term is found in the field, it will match but be scored lower than phrase.
            // If you would like ALL terms to match, use MUST instead of SHOULD.
            var wordQuery = queryBuilder.CreateBooleanQuery(C.FIELD_NAME, searchText, Occur.SHOULD);
            wordQuery.Boost = 3;
            query.Add(wordQuery, Occur.SHOULD);

            // Append a wildcard to the last term for incomplete text matches.
            var lastTerm = searchText.Split(" ").LastOrDefault();

            if (!string.IsNullOrWhiteSpace(lastTerm))
            {
                var lastWildcard = new WildcardQuery(new Term(C.FIELD_NAME, lastTerm + "*"))
                {
                    Boost = 3
                };

                query.Add(lastWildcard, Occur.SHOULD);
            }
        }

        if (query.Clauses.Count < 1)
        {
            return new MatchAllDocsQuery();
        }

        return query;
    }

    private static QueryWrapperFilter BuildFilter(SearchRecipesRequest request, FacetsConfig facetsConfig)
    {
        // Used when other filters are AND'd between fields, but OR'd within the field.
        // Otherwise, the field needs to add it's own filter to the outerQuery.
        var mainDrillDownQuery = new DrillDownQuery(facetsConfig);

        var outerQuery = new BooleanQuery()
        {
            { mainDrillDownQuery, Occur.MUST }
        };

        if (request.IsForMealPlanning is not null)
        {
            mainDrillDownQuery.Add(C.FIELD_IS_FOR_MEAL_PLANNING, request.IsForMealPlanning.ToString());
        }

        if (request.CategoryIds?.Length > 0)
        {
            if (!request.AllCategories)
            {
                // OR - matches recipes with ANY of the selected categories
                foreach (var categoryId in request.CategoryIds)
                {
                    mainDrillDownQuery.Add(C.FIELD_CATEGORY_IDS, categoryId.ToString());
                }
            }
            else
            {
                // AND - matches recipes with ALL of the selected categories
                foreach (var categoryId in request.CategoryIds)
                {
                    var singleCategoryQuery = new DrillDownQuery(facetsConfig)
                    {
                        { C.FIELD_CATEGORY_IDS, categoryId.ToString() }
                    };

                    outerQuery.Add(singleCategoryQuery, Occur.MUST);
                }
            }
        }

        if ((mainDrillDownQuery as IEnumerable<BooleanClause>).Any() || outerQuery.Clauses.Count > 1)
        {
            return new QueryWrapperFilter(outerQuery);
        }

        // No filters selected.
        return new QueryWrapperFilter(new MatchAllDocsQuery());
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
