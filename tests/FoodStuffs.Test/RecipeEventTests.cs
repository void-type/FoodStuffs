using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
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
    public async Task GetRecipe_returns_failure_when_recipe_does_not_exist()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var result = await new GetRecipeHandler(context)
            .Handle(new GetRecipeRequest(-22));

        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task DeleteRecipe_deletes_recipe_and_returns_id_when_recipe_exists()
    {
        // Due to the way we delete, we need a fresh dbcontext to remove tracked entities.
        await using var context1 = Deps.FoodStuffsContext("delete images").Seed();

        await using var context = Deps.FoodStuffsContext("delete images");

        var recipeToDelete = context.Recipes
            .Include(r => r.Images)
            .ThenInclude(r => r.ImageBlob)
            .AsNoTracking()
            .First(r => r.Name == "Cheeseburger");

        // For testing, we need to pull in all entities so EF can cascade delete.
        // In prod, SQL Server will do the cascading without needing to bring them into memory.
        var _ = context.Images.Include(x => x.ImageBlob).ToList();

        Assert.True(recipeToDelete.Images.Count != 0);
        Assert.True(recipeToDelete.Images.Select(i => i.ImageBlob).Any());
        Assert.Equal(recipeToDelete.Images.Count, recipeToDelete.Images.Select(i => i.ImageBlob).Count());

        var indexService = Substitute.For<IRecipeIndexService>();

        var result = await new DeleteRecipeHandler(context, indexService)
            .Handle(new DeleteRecipeRequest(recipeToDelete.Id));

        Assert.True(result.IsSuccess);

        Assert.Equal(recipeToDelete.Id, result.Value.Id);

        var imageIds = recipeToDelete.Images.Select(i => i.Id);

        Assert.Empty(context.Images.Where(i => imageIds.Contains(i.Id)).AsNoTracking().ToList());
    }

    [Fact]
    public async Task DeleteRecipe_returns_failure_when_recipe_does_not_exist()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var indexService = Substitute.For<IRecipeIndexService>();

        var result = await new DeleteRecipeHandler(context, indexService)
            .Handle(new DeleteRecipeRequest(-22));

        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task SaveRecipe_creates_new_recipe_when_id_0_is_specified()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var indexService = Substitute.For<IRecipeIndexService>();

        var result = await new SaveRecipeHandler(context, indexService)
            .Handle(new SaveRecipeRequest(0, "New", "New", string.Empty, null, 20, false, [new SaveRecipeRequestIngredient("New", 1, 1, false)], [], ["Category2", "Category3", "Category4"]));

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Id > 0);

        var recipe = context.Recipes.Find(result.Value.Id);

        Assert.Equal(Deps.DateTimeServiceLate.Moment, recipe.CreatedOn);
        Assert.Equal(Deps.DateTimeServiceLate.Moment, recipe.ModifiedOn);
        Assert.DoesNotContain("Category1", recipe.Categories.Select(c => c.Name));
        Assert.Contains("Category2", recipe.Categories.Select(c => c.Name));
        Assert.Contains("Category3", recipe.Categories.Select(c => c.Name));
        Assert.Contains("Category4", recipe.Categories.Select(c => c.Name));
    }

    [Fact]
    public async Task SaveRecipe_updates_existing_recipe_when_exists()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var existingRecipeId = context.Recipes.First().Id;

        var indexService = Substitute.For<IRecipeIndexService>();

        var result = await new SaveRecipeHandler(context, indexService)
            .Handle(new SaveRecipeRequest(existingRecipeId, "New", "New", string.Empty, null, 20, false, [new SaveRecipeRequestIngredient("New", 1, 1, false)], [], ["Category2", "Category3", "Category4"]));

        Assert.True(result.IsSuccess);
        Assert.Equal(existingRecipeId, result.Value.Id);

        var updatedRecipe = context.Recipes.Find(existingRecipeId);
        Assert.Equal(Deps.DateTimeServiceLate.Moment, updatedRecipe.ModifiedOn);
        Assert.DoesNotContain("Category1", updatedRecipe.Categories.Select(c => c.Name));
        Assert.Contains("Category2", updatedRecipe.Categories.Select(c => c.Name));
        Assert.Contains("Category3", updatedRecipe.Categories.Select(c => c.Name));
        Assert.Contains("Category4", updatedRecipe.Categories.Select(c => c.Name));
    }
}
