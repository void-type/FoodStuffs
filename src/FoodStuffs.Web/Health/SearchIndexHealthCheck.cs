using FoodStuffs.Model.Search;
using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.Lucene;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FoodStuffs.Web.Health;

/// <summary>
/// Verifies that the Lucene indexes can be opened. Missing, corrupt or rebuilding indexes report unhealthy.
/// </summary>
public class SearchIndexHealthCheck(SearchSettings settings) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        var counts = new List<string>();

        try
        {
            foreach (var indexName in new[] { RecipeSearchConstants.INDEX_NAME, GroceryItemSearchConstants.INDEX_NAME })
            {
                using var readers = new LuceneReaders(settings, indexName);
                counts.Add($"{indexName}: {readers.IndexReader.NumDocs} documents");
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("A search index could not be opened.", ex);
        }

        return HealthCheckResult.Healthy(string.Join(", ", counts));
    }
}
