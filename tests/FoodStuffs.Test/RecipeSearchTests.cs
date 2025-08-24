using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Search;
using FoodStuffs.Model.Search.Recipes;
using FoodStuffs.Model.Search.Recipes.Models;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VoidCore.Model.Time;
using Xunit;

namespace FoodStuffs.Test;

public class RecipeSearchTests : IAsyncLifetime
{
    private IRecipeQueryService? _queryService;

    private IRecipeQueryService QueryService => _queryService ?? throw new InvalidOperationException("QueryService not initialized.");

    public async Task InitializeAsync()
    {
        _queryService = await BuildQueryServiceAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    private static async Task<IRecipeQueryService> BuildQueryServiceAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var logger = Substitute.For<ILogger<RecipeIndexService>>();

        if (Directory.Exists("App_Data/Lucene"))
        {
            Directory.Delete("App_Data/Lucene", true);
        }

        var settings = new SearchSettings();

        var indexService = new RecipeIndexService(logger, settings, context);
        await indexService.RebuildAsync(CancellationToken.None);

        return new RecipeQueryService(settings, new UtcNowDateTimeService());
    }

    [Fact]
    public async Task SearchRecipes_returns_a_page_of_recipesAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var result = await new SearchRecipesHandler(QueryService)
            .Handle(new SearchRecipesRequest(null, null, false, null, null, null, true, 2, 1));

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Results.Count);
        Assert.Equal(3, result.Value.Results.TotalCount);
        Assert.Equal(2, result.Value.Results.Page);
        Assert.Equal(1, result.Value.Results.Take);
    }

    [Fact]
    public async Task SearchRecipes_returns_all_recipes_when_paging_is_disabledAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var result = await new SearchRecipesHandler(QueryService)
            .Handle(new SearchRecipesRequest(null, null, false, null, null, null, false, 0, 0));

        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Results.Count);
        Assert.Equal(3, result.Value.Results.TotalCount);
        Assert.Contains("Cheeseburger", result.Value.Results.Items.Select(r => r.Name));
        Assert.Contains("Hotdog", result.Value.Results.Items.Select(r => r.Name));
        Assert.Contains("Sandwich", result.Value.Results.Items.Select(r => r.Name));
        Assert.Contains("Category1", result.Value.Results.Items.SelectMany(r => r.Categories.Select(c => c.Name)));
    }

    [Fact]
    public async Task SearchRecipes_can_sort_by_descendingAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var result = await new SearchRecipesHandler(QueryService)
            .Handle(new SearchRecipesRequest(null, null, false, null, "z-a", null, true, 1, 1));

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Results.Count);
        Assert.Equal(3, result.Value.Results.TotalCount);
        Assert.Equal(1, result.Value.Results.Page);
        Assert.Equal(1, result.Value.Results.Take);
        Assert.Equal("Sandwich", result.Value.Results.Items.First().Name);
    }

    [Fact]
    public async Task SearchRecipes_can_sort_by_ascendingAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var result = await new SearchRecipesHandler(QueryService)
            .Handle(new SearchRecipesRequest(null, null, false, null, "a-z", null, true, 1, 1));

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Results.Count);
        Assert.Equal(3, result.Value.Results.TotalCount);
        Assert.Equal(1, result.Value.Results.Page);
        Assert.Equal(1, result.Value.Results.Take);
        Assert.Contains("Cheeseburger", result.Value.Results.Items.First().Name);
    }

    [Fact]
    public async Task SearchRecipes_can_search_by_recipe_nameAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var result = await new SearchRecipesHandler(QueryService)
            .Handle(new SearchRecipesRequest("Hutdug", null, false, null, null, null, true, 1, 2));

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Results.Count);
        Assert.Equal(1, result.Value.Results.TotalCount);
        Assert.Equal(1, result.Value.Results.Page);
        Assert.Equal(2, result.Value.Results.Take);
        Assert.Contains("Hotdog", result.Value.Results.Items.Select(r => r.Name));
    }

    [Fact]
    public async Task SearchRecipes_can_search_by_category_nameAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var cat = context.Categories.ToList();

        var result = await new SearchRecipesHandler(QueryService)
            .Handle(new SearchRecipesRequest(null, [1, 2, 3], false, null, null, null, true, 1, 4));

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Results.Count);
        Assert.Equal(2, result.Value.Results.TotalCount);
        Assert.Equal(1, result.Value.Results.Page);
        Assert.Equal(4, result.Value.Results.Take);
        Assert.Contains("Cheeseburger", result.Value.Results.Items.Select(r => r.Name));
        Assert.Contains("Hotdog", result.Value.Results.Items.Select(r => r.Name));
        Assert.DoesNotContain("Sandwich", result.Value.Results.Items.Select(r => r.Name));
    }

    [Fact]
    public async Task SearchRecipes_can_search_by_category_all_nameAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var cat = context.Categories.ToList();

        var result = await new SearchRecipesHandler(QueryService)
            .Handle(new SearchRecipesRequest(null, [1, 4], true, null, null, null, true, 1, 4));

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Results.Count);
        Assert.Equal(1, result.Value.Results.TotalCount);
        Assert.Equal(1, result.Value.Results.Page);
        Assert.Equal(4, result.Value.Results.Take);
        Assert.Contains("Cheeseburger", result.Value.Results.Items.Select(r => r.Name));
        Assert.DoesNotContain("Hotdog", result.Value.Results.Items.Select(r => r.Name));
        Assert.DoesNotContain("Sandwich", result.Value.Results.Items.Select(r => r.Name));
    }

    [Fact]
    public async Task SearchRecipes_can_search_by_is_for_meal_planningAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var result = await new SearchRecipesHandler(QueryService)
            .Handle(new SearchRecipesRequest(null, null, false, true, null, null, true, 1, 4));

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Results.Count);
        Assert.Equal(1, result.Value.Results.TotalCount);
        Assert.Equal(1, result.Value.Results.Page);
        Assert.Equal(4, result.Value.Results.Take);
        Assert.DoesNotContain("Cheeseburger", result.Value.Results.Items.Select(r => r.Name));
        Assert.Contains("Hotdog", result.Value.Results.Items.Select(r => r.Name));
        Assert.DoesNotContain("Sandwich", result.Value.Results.Items.Select(r => r.Name));
    }

    [Fact]
    public async Task SearchRecipes_returns_empty_item_set_when_name_search_matches_zero_itemsAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var result = await new SearchRecipesHandler(QueryService)
            .Handle(new SearchRecipesRequest("nothing matches", null, false, null, null, null, true, 1, 2));

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.Results.Count);
        Assert.Equal(0, result.Value.Results.TotalCount);
    }

    [Fact]
    public async Task SearchRecipes_returns_empty_item_set_when_category_search_matches_zero_itemsAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var result = await new SearchRecipesHandler(QueryService)
            .Handle(new SearchRecipesRequest(null, [1000, 2000, 3000], false, null, null, null, true, 1, 2));

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.Results.Count);
        Assert.Equal(0, result.Value.Results.TotalCount);
    }
}
