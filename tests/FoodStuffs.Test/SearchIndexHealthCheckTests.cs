using FoodStuffs.Model.Search;
using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.Recipes;
using FoodStuffs.Web.Health;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FoodStuffs.Test;

public class SearchIndexHealthCheckTests : IDisposable
{
    private readonly string _indexFolder = $"App_Data/LuceneHealth{Guid.NewGuid():N}";

    public void Dispose()
    {
        if (Directory.Exists(_indexFolder))
        {
            Directory.Delete(_indexFolder, true);
        }

        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task Check_is_unhealthy_when_indexes_are_missingAsync()
    {
        var check = new SearchIndexHealthCheck(new SearchSettings { IndexFolder = _indexFolder });

        var result = await check.CheckHealthAsync(new HealthCheckContext());

        Assert.Equal(HealthStatus.Unhealthy, result.Status);
        Assert.NotNull(result.Exception);
    }

    [Fact]
    public async Task Check_is_healthy_when_indexes_are_builtAsync()
    {
        var settings = new SearchSettings { IndexFolder = _indexFolder };
        await using var context = Deps.FoodStuffsContext().Seed();

        await new RecipeIndexService(Substitute.For<ILogger<RecipeIndexService>>(), settings, context)
            .RebuildAsync(CancellationToken.None);
        await new GroceryItemIndexService(Substitute.For<ILogger<GroceryItemIndexService>>(), settings, context)
            .RebuildAsync(CancellationToken.None);

        var result = await new SearchIndexHealthCheck(settings).CheckHealthAsync(new HealthCheckContext());

        Assert.Equal(HealthStatus.Healthy, result.Status);
        Assert.Contains("Recipe: 3 documents", result.Description);
    }
}
