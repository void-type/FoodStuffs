using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Events.Recipes.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using VoidCore.EntityFramework;
using Xunit;

namespace FoodStuffs.Test;

public class RecipeEventTests
{
    [Fact]
    public async Task GetRecipe_returns_a_recipe_when_recipe_exists()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var recipeToFind = context.Recipes.First();

        var result = await new GetRecipeHandler(context)
            .Handle(new GetRecipeRequest(recipeToFind.Id));

        Assert.True(result.IsSuccess);
        Assert.Equal(recipeToFind.Id, result.Value.Id);
        Assert.Equal(recipeToFind.Name, result.Value.Name);
    }

    [Fact]
    public async Task GetRecipe_returns_failure_when_recipe_does_not_existAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var result = await new GetRecipeHandler(context)
            .Handle(new GetRecipeRequest(-22));

        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task DeleteRecipe_moves_recipe_to_the_recycle_bin_when_recipe_existsAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var recipeToDelete = context.Recipes
            .Include(r => r.Images)
            .First(r => r.Name == "Cheeseburger");

        var indexService = Substitute.For<ISearchIndexService>();

        var result = await new DeleteRecipeHandler(context, indexService)
            .Handle(new DeleteRecipeRequest(recipeToDelete.Id));

        Assert.True(result.IsSuccess);
        Assert.Equal(recipeToDelete.Id, result.Value.Id);

        await indexService.Received(1).RemoveAsync(SearchIndex.Recipes, recipeToDelete.Id, Arg.Any<CancellationToken>());

        var deletedRecipe = await context.Recipes
            .OnlySoftDeleted()
            .AsNoTracking()
            .Include(r => r.Images)
            .SingleAsync(r => r.Id == recipeToDelete.Id);

        Assert.True(deletedRecipe.IsDeleted);
        Assert.Equal(Deps.Late, deletedRecipe.DeletedOn);
        Assert.Equal(Deps.CurrentUserAccessor.GetUserName(), deletedRecipe.DeletedBy);
        Assert.Equal(recipeToDelete.Images.Count, deletedRecipe.Images.Count);
    }

    [Fact]
    public async Task DeleteRecipe_returns_failure_when_recipe_does_not_existAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var indexService = Substitute.For<ISearchIndexService>();

        var result = await new DeleteRecipeHandler(context, indexService)
            .Handle(new DeleteRecipeRequest(-22));

        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task SaveRecipe_creates_new_recipe_when_id_0_is_specifiedAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var indexService = Substitute.For<ISearchIndexService>();

        var result = await new SaveRecipeHandler(context, new SaveRecipeRequestValidator(), indexService)
            .Handle(new SaveRecipeRequest(0, "New", "New", string.Empty, null, 20, false, 2, [], ["Category2", "Category3", "Category4"]));

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Entity.Id > 0);

        var recipe = await context.Recipes.FindAsync(result.Value.Entity.Id);

        Assert.NotNull(recipe);

        Assert.Equal(Deps.Late, recipe.CreatedOn);
        Assert.Equal(Deps.Late, recipe.ModifiedOn);
        Assert.DoesNotContain("Category1", recipe.Categories.Select(c => c.Name));
        Assert.Contains("Category2", recipe.Categories.Select(c => c.Name));
        Assert.Contains("Category3", recipe.Categories.Select(c => c.Name));
        Assert.Contains("Category4", recipe.Categories.Select(c => c.Name));
    }

    [Fact]
    public async Task SaveRecipe_updates_existing_recipe_when_existsAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var existingRecipeId = context.Recipes.First().Id;

        var indexService = Substitute.For<ISearchIndexService>();

        var result = await new SaveRecipeHandler(context, new SaveRecipeRequestValidator(), indexService)
            .Handle(new SaveRecipeRequest(existingRecipeId, "New", "New", string.Empty, null, 20, false, 2, [], ["Category2", "Category3", "Category4"]));

        Assert.True(result.IsSuccess);
        Assert.Equal(existingRecipeId, result.Value.Entity.Id);

        var updatedRecipe = await context.Recipes.FindAsync(existingRecipeId);
        Assert.NotNull(updatedRecipe);

        Assert.Equal(Deps.Late, updatedRecipe.ModifiedOn);
        Assert.DoesNotContain("Category1", updatedRecipe.Categories.Select(c => c.Name));
        Assert.Contains("Category2", updatedRecipe.Categories.Select(c => c.Name));
        Assert.Contains("Category3", updatedRecipe.Categories.Select(c => c.Name));
        Assert.Contains("Category4", updatedRecipe.Categories.Select(c => c.Name));
    }
}
