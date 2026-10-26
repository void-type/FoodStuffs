using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Events.Categories;
using FoodStuffs.Model.Events.Categories.Models;
using FoodStuffs.Model.Events.MealPlans;
using FoodStuffs.Model.Events.MealPlans.Models;
using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Events.Recipes.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using VoidCore.EntityFramework;
using Xunit;

namespace FoodStuffs.Test;

public class RecipeRecycleBinTests
{
    [Fact]
    public async Task DeleteRecipe_hides_the_recipe_from_queriesAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var recipe = context.Recipes.First(r => r.Name == "Hotdog");

        await DeleteAsync(context, recipe.Id);

        var result = await new GetRecipeHandler(context).Handle(new GetRecipeRequest(recipe.Id));

        Assert.True(result.IsFailed);
        Assert.DoesNotContain(context.Recipes, r => r.Id == recipe.Id);
    }

    [Fact]
    public async Task DeleteRecipe_hides_the_recipe_from_meal_plans_and_restoring_puts_it_backAsync()
    {
        await using var context = Deps.FoodStuffsContext("meal plan").Seed();

        var recipes = context.Recipes.OrderBy(r => r.Id).ToList();
        var mealPlan = new MealPlan { Name = "Plan" };
        mealPlan.RecipeRelations.AddRange(recipes.Select((r, i) => new MealPlanRecipeRelation { Recipe = r, Order = i }));
        context.MealPlans.Add(mealPlan);
        await context.SaveChangesAsync();

        await DeleteAsync(context, recipes[0].Id);

        // Use fresh contexts so the meal plan is loaded the way a request would.
        await using (var readContext = Deps.FoodStuffsContext("meal plan"))
        {
            var result = await new GetMealPlanHandler(readContext).Handle(new GetMealPlanRequest(mealPlan.Id));
            var list = await new ListMealPlansHandler(readContext).Handle(new ListMealPlansRequest(true, 1, 30));

            Assert.True(result.IsSuccess);
            Assert.Equal(recipes.Count - 1, result.Value.Recipes.Count);
            Assert.DoesNotContain(result.Value.Recipes, r => r.Id == recipes[0].Id);
            Assert.Equal(recipes.Count - 1, list.Value.Items.Single().RecipeCount);
        }

        var restoreResult = await new RestoreRecipeHandler(context, Substitute.For<ISearchIndexService>())
            .Handle(new RestoreRecipeRequest(recipes[0].Id));

        Assert.True(restoreResult.IsSuccess);

        await using (var readContext = Deps.FoodStuffsContext("meal plan"))
        {
            var result = await new GetMealPlanHandler(readContext).Handle(new GetMealPlanRequest(mealPlan.Id));
            var list = await new ListMealPlansHandler(readContext).Handle(new ListMealPlansRequest(true, 1, 30));

            Assert.Equal(recipes.Count, result.Value.Recipes.Count);
            Assert.Equal(recipes.Count, list.Value.Items.Single().RecipeCount);
        }
    }

    [Fact]
    public async Task DeleteRecipe_excludes_the_recipe_from_category_countsAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var recipe = context.Recipes.First(r => r.Name == "Cheeseburger");

        var request = new ListCategoriesRequest(null, null, null, false, false, 1, 30);

        var before = await new ListCategoriesHandler(context).Handle(request);
        Assert.Equal(2, before.Value.Items.Single(c => c.Name == "Category4").RecipeCount);

        await DeleteAsync(context, recipe.Id);

        var after = await new ListCategoriesHandler(context).Handle(request);
        Assert.Equal(1, after.Value.Items.Single(c => c.Name == "Category4").RecipeCount);
    }

    [Fact]
    public async Task DeleteRecipe_reindexes_related_grocery_itemsAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var groceryItem = new GroceryItem { Name = "Bun" };
        var recipe = context.Recipes.First(r => r.Name == "Cheeseburger");
        recipe.GroceryItemRelations.Add(new RecipeGroceryItemRelation { GroceryItem = groceryItem, Quantity = 1 });
        await context.SaveChangesAsync();

        var indexService = Substitute.For<ISearchIndexService>();

        await new DeleteRecipeHandler(context, indexService).Handle(new DeleteRecipeRequest(recipe.Id));

        indexService.Received(1).EnqueueUpdate(SearchIndex.GroceryItems, Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(new[] { groceryItem.Id })));
    }

    [Fact]
    public async Task ListDeletedRecipes_returns_only_deleted_recipesAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var recipe = context.Recipes.First(r => r.Name == "Hotdog");

        await DeleteAsync(context, recipe.Id);

        var result = await new ListDeletedRecipesHandler(context).Handle(new ListDeletedRecipesRequest(true, 1, 30));

        Assert.True(result.IsSuccess);
        var item = Assert.Single(result.Value.Items);
        Assert.Equal(recipe.Id, item.Id);
        Assert.Equal("Hotdog", item.Name);
        Assert.Equal(Deps.Late, item.DeletedOn);
        Assert.Equal(Deps.CurrentUserAccessor.GetUserName(), item.DeletedBy);
    }

    [Fact]
    public async Task RestoreRecipe_makes_the_recipe_visible_again_and_reindexes_itAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var recipe = context.Recipes.First(r => r.Name == "Hotdog");

        await DeleteAsync(context, recipe.Id);

        var indexService = Substitute.For<ISearchIndexService>();

        var result = await new RestoreRecipeHandler(context, indexService).Handle(new RestoreRecipeRequest(recipe.Id));

        Assert.True(result.IsSuccess);

        var restored = await context.Recipes.AsNoTracking().SingleAsync(r => r.Id == recipe.Id);
        Assert.False(restored.IsDeleted);
        Assert.Null(restored.DeletedOn);
        Assert.Empty(restored.DeletedBy);

        await indexService.Received(1).UpdateAsync(SearchIndex.Recipes, recipe.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RestoreRecipe_fails_when_another_recipe_took_the_nameAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var recipe = context.Recipes.First(r => r.Name == "Hotdog");

        await DeleteAsync(context, recipe.Id);

        var indexService = Substitute.For<ISearchIndexService>();

        var saveResult = await new SaveRecipeHandler(context, new SaveRecipeRequestValidator(), indexService)
            .Handle(new SaveRecipeRequest(0, "Hotdog", "New", string.Empty, null, 20, false, 2, [], []));

        Assert.True(saveResult.IsSuccess);

        var result = await new RestoreRecipeHandler(context, indexService).Handle(new RestoreRecipeRequest(recipe.Id));

        Assert.True(result.IsFailed);
        Assert.True(await context.Recipes.OnlySoftDeleted().AnyAsync(r => r.Id == recipe.Id));
    }

    [Fact]
    public async Task RestoreRecipe_fails_when_recipe_is_not_in_the_recycle_binAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var recipe = context.Recipes.First();

        var result = await new RestoreRecipeHandler(context, Substitute.For<ISearchIndexService>())
            .Handle(new RestoreRecipeRequest(recipe.Id));

        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task PermanentlyDeleteRecipe_removes_the_recipe_and_its_images_from_the_recycle_binAsync()
    {
        // Due to the way we delete, we need a fresh dbcontext to remove tracked entities.
        await using var seedContext = Deps.FoodStuffsContext("permanently delete").Seed();

        await using var context = Deps.FoodStuffsContext("permanently delete");

        var recipe = context.Recipes
            .Include(r => r.Images)
            .AsNoTracking()
            .First(r => r.Name == "Cheeseburger");

        var imageIds = recipe.Images.Select(i => i.Id).ToList();
        Assert.NotEmpty(imageIds);

        await DeleteAsync(context, recipe.Id);

        // For testing, we need to pull in all entities so EF can cascade delete.
        // In prod, SQL Server will do the cascading without needing to bring them into memory.
        var _ = context.Images.Include(x => x.ImageBlob).ToList();

        var result = await new PermanentlyDeleteRecipeHandler(context)
            .Handle(new PermanentlyDeleteRecipeRequest(recipe.Id));

        Assert.True(result.IsSuccess);
        Assert.False(await context.Recipes.IncludeSoftDeleted().AnyAsync(r => r.Id == recipe.Id));
        Assert.Empty(context.Images.Where(i => imageIds.Contains(i.Id)).AsNoTracking().ToList());
    }

    [Fact]
    public async Task PermanentlyDeleteRecipe_fails_when_recipe_is_not_in_the_recycle_binAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var recipe = context.Recipes.First();

        var result = await new PermanentlyDeleteRecipeHandler(context)
            .Handle(new PermanentlyDeleteRecipeRequest(recipe.Id));

        Assert.True(result.IsFailed);
        Assert.True(await context.Recipes.AnyAsync(r => r.Id == recipe.Id));
    }

    [Fact]
    public async Task Removing_a_recipe_that_is_not_in_the_recycle_bin_throwsAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        context.Recipes.Remove(context.Recipes.First());

        await Assert.ThrowsAsync<InvalidOperationException>(() => context.SaveChangesAsync());
    }

    private static async Task DeleteAsync(FoodStuffs.Model.Data.FoodStuffsContext context, int recipeId)
    {
        var result = await new DeleteRecipeHandler(context, Substitute.For<ISearchIndexService>())
            .Handle(new DeleteRecipeRequest(recipeId));

        Assert.True(result.IsSuccess);
    }
}
