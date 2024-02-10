using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Search;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VoidCore.Model.Time;
using Xunit;

namespace FoodStuffs.Test;

public class RecipeSearchTests : IAsyncLifetime
{
    private IRecipeQueryService _queryService;

    public async Task InitializeAsync()
    {
        _queryService = await BuildQueryService();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    private static async Task<IRecipeQueryService> BuildQueryService()
    {
        await using var context = Deps.FoodStuffsContext().Seed();
        var data = context.FoodStuffsData();

        var logger = Substitute.For<ILogger<RecipeIndexService>>();

        if (Directory.Exists("App_Data/Lucene"))
        {
            Directory.Delete("App_Data/Lucene", true);
        }

        var settings = new RecipeSearchSettings
        {
            IndexFolder = "App_Data/Lucene/RecipeIndex",
            TaxonomyFolder = "App_Data/Lucene/RecipeTaxonomy",
        };

        var indexService = new RecipeIndexService(logger, settings, context);
        await indexService.Rebuild(CancellationToken.None);

        return new RecipeQueryService(settings, new UtcNowDateTimeService());
    }

    [Fact]
    public async Task SearchRecipes_returns_a_page_of_recipes()
    {
        await using var context = Deps.FoodStuffsContext().Seed();
        var data = context.FoodStuffsData();

        var result = await new SearchRecipesHandler(_queryService)
            .Handle(new SearchRecipesRequest(null, null, null, null, true, 2, 1));

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Count);
        Assert.Equal(3, result.Value.TotalCount);
        Assert.Equal(2, result.Value.Page);
        Assert.Equal(1, result.Value.Take);
    }

    [Fact]
    public async Task SearchRecipes_returns_all_recipes_when_paging_is_disabled()
    {
        await using var context = Deps.FoodStuffsContext().Seed();
        var data = context.FoodStuffsData();

        var result = await new SearchRecipesHandler(_queryService)
            .Handle(new SearchRecipesRequest(null, null, null, null, false, 0, 0));

        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Count);
        Assert.Equal(3, result.Value.TotalCount);
        Assert.Contains("Cheeseburger", result.Value.Items.Select(r => r.Name));
        Assert.Contains("Hotdog", result.Value.Items.Select(r => r.Name));
        Assert.Contains("Sandwich", result.Value.Items.Select(r => r.Name));
        Assert.Contains("Category1", result.Value.Items.SelectMany(r => r.Categories));
    }

    [Fact]
    public async Task SearchRecipes_can_sort_by_descending()
    {
        await using var context = Deps.FoodStuffsContext().Seed();
        var data = context.FoodStuffsData();

        var result = await new SearchRecipesHandler(_queryService)
            .Handle(new SearchRecipesRequest(null, null, null, "z-a", true, 1, 1));

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Count);
        Assert.Equal(3, result.Value.TotalCount);
        Assert.Equal(1, result.Value.Page);
        Assert.Equal(1, result.Value.Take);
        Assert.Equal("Sandwich", result.Value.Items.First().Name);
    }

    [Fact]
    public async Task SearchRecipes_can_sort_by_ascending()
    {
        await using var context = Deps.FoodStuffsContext().Seed();
        var data = context.FoodStuffsData();

        var result = await new SearchRecipesHandler(_queryService)
            .Handle(new SearchRecipesRequest(null, null, null, "a-z", true, 1, 1));

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Count);
        Assert.Equal(3, result.Value.TotalCount);
        Assert.Equal(1, result.Value.Page);
        Assert.Equal(1, result.Value.Take);
        Assert.Contains("Cheeseburger", result.Value.Items.First().Name);
    }

    [Fact]
    public async Task SearchRecipes_can_search_by_recipe_name()
    {
        await using var context = Deps.FoodStuffsContext().Seed();
        var data = context.FoodStuffsData();

        var result = await new SearchRecipesHandler(_queryService)
            .Handle(new SearchRecipesRequest("Hutdug", null, null, null, true, 1, 2));

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Count);
        Assert.Equal(1, result.Value.TotalCount);
        Assert.Equal(1, result.Value.Page);
        Assert.Equal(2, result.Value.Take);
        Assert.Contains("Hotdog", result.Value.Items.Select(r => r.Name));
    }

    [Fact]
    public async Task SearchRecipes_can_search_by_category_name()
    {
        await using var context = Deps.FoodStuffsContext().Seed();
        var data = context.FoodStuffsData();

        var result = await new SearchRecipesHandler(_queryService)
            .Handle(new SearchRecipesRequest(null, [1, 2, 3], null, null, true, 1, 4));

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);
        Assert.Equal(2, result.Value.TotalCount);
        Assert.Equal(1, result.Value.Page);
        Assert.Equal(4, result.Value.Take);
        Assert.Contains("Cheeseburger", result.Value.Items.Select(r => r.Name));
        Assert.Contains("Hotdog", result.Value.Items.Select(r => r.Name));
        Assert.DoesNotContain("Sandwich", result.Value.Items.Select(r => r.Name));
    }

    [Fact]
    public async Task SearchRecipes_can_search_by_is_for_meal_planning()
    {
        await using var context = Deps.FoodStuffsContext().Seed();
        var data = context.FoodStuffsData();

        var result = await new SearchRecipesHandler(_queryService)
            .Handle(new SearchRecipesRequest(null, null, true, null, true, 1, 4));

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Count);
        Assert.Equal(1, result.Value.TotalCount);
        Assert.Equal(1, result.Value.Page);
        Assert.Equal(4, result.Value.Take);
        Assert.DoesNotContain("Cheeseburger", result.Value.Items.Select(r => r.Name));
        Assert.Contains("Hotdog", result.Value.Items.Select(r => r.Name));
        Assert.DoesNotContain("Sandwich", result.Value.Items.Select(r => r.Name));
    }

    [Fact]
    public async Task SearchRecipes_returns_empty_item_set_when_name_search_matches_zero_items()
    {
        await using var context = Deps.FoodStuffsContext().Seed();
        var data = context.FoodStuffsData();

        var result = await new SearchRecipesHandler(_queryService)
            .Handle(new SearchRecipesRequest("nothing matches", null, null, null, true, 1, 2));

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.Count);
        Assert.Equal(0, result.Value.TotalCount);
    }

    [Fact]
    public async Task SearchRecipes_returns_empty_item_set_when_category_search_matches_zero_items()
    {
        await using var context = Deps.FoodStuffsContext().Seed();
        var data = context.FoodStuffsData();

        var result = await new SearchRecipesHandler(_queryService)
            .Handle(new SearchRecipesRequest(null, [1000, 2000, 3000], null, null, true, 1, 2));

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.Count);
        Assert.Equal(0, result.Value.TotalCount);
    }
}
