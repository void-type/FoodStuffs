using System.Linq;
using System.Threading.Tasks;
using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.Queries;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FoodStuffs.Test.Model.Events
{
    public class RecipeEventTests
    {
        [Fact]
        public async Task GetRecipe_returns_a_recipe_when_recipe_exists()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var recipes = await data.Recipes.ListAll(default);
            var recipeToFind = recipes.First();

            var result = await new GetRecipe.Handler(data)
                .Handle(new GetRecipe.Request(recipeToFind.Id));

            Assert.True(result.IsSuccess);
            Assert.Equal(recipeToFind.Id, result.Value.Id);
            Assert.Equal(recipeToFind.Name, result.Value.Name);
        }

        [Fact]
        public async Task GetRecipe_returns_failure_when_recipe_does_not_exist()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var result = await new GetRecipe.Handler(data)
                .Handle(new GetRecipe.Request(-22));

            Assert.True(result.IsFailed);
        }

        [Fact]
        public async Task ListRecipes_returns_a_page_of_recipes()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, null, null, true, 2, 1));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(3, result.Value.TotalCount);
            Assert.Equal(2, result.Value.Page);
            Assert.Equal(1, result.Value.Take);
        }

        [Fact]
        public async Task ListRecipe_returns_all_recipes_when_paging_is_disabled()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, null, null, false, 0, 0));

            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Value.Count);
            Assert.Equal(3, result.Value.TotalCount);
            Assert.Contains("Recipe1", result.Value.Items.Select(r => r.Name));
            Assert.Contains("Recipe2", result.Value.Items.Select(r => r.Name));
            Assert.Contains("Recipe3", result.Value.Items.Select(r => r.Name));
            Assert.Contains("Category1", result.Value.Items.SelectMany(r => r.Categories));
        }

        [Fact]
        public async Task ListRecipes_can_sort_by_descending()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, null, "nameDesc", true, 1, 1));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(3, result.Value.TotalCount);
            Assert.Equal(1, result.Value.Page);
            Assert.Equal(1, result.Value.Take);
            Assert.Equal("Recipe3", result.Value.Items.First().Name);
        }

        [Fact]
        public async Task ListRecipes_can_sort_by_ascending()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, null, "name", true, 1, 1));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(3, result.Value.TotalCount);
            Assert.Equal(1, result.Value.Page);
            Assert.Equal(1, result.Value.Take);
            Assert.Contains("Recipe1", result.Value.Items.First().Name);
        }

        [Fact]
        public async Task ListRecipes_can_search_by_recipe_name()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request("recipe2", null, null, true, 1, 2));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(1, result.Value.TotalCount);
            Assert.Equal(1, result.Value.Page);
            Assert.Equal(2, result.Value.Take);
            Assert.Contains("Recipe2", result.Value.Items.Select(r => r.Name));
        }

        [Fact]
        public async Task ListRecipes_can_search_by_category_name()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, "cat", null, true, 1, 4));

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);
            Assert.Equal(2, result.Value.TotalCount);
            Assert.Equal(1, result.Value.Page);
            Assert.Equal(4, result.Value.Take);
            Assert.Contains("Recipe1", result.Value.Items.Select(r => r.Name));
            Assert.Contains("Recipe2", result.Value.Items.Select(r => r.Name));
            Assert.DoesNotContain("Recipe3", result.Value.Items.Select(r => r.Name));
        }

        [Fact]
        public async Task ListRecipes_returns_empty_item_set_when_name_search_matches_zero_items()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request("nothing matches", null, null, true, 1, 2));

            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value.Count);
            Assert.Equal(0, result.Value.TotalCount);
        }

        [Fact]
        public async Task ListRecipes_returns_empty_item_set_when_category_search_matches_zero_items()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, "nothing matches", null, true, 1, 2));

            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value.Count);
            Assert.Equal(0, result.Value.TotalCount);
        }

        [Fact]
        public async Task DeleteRecipe_deletes_recipe_and_returns_id_when_recipe_exists()
        {
            // Due to the way we delete, we need a fresh dbcontext to remove tracked entities.
            await using var context1 = Deps.FoodStuffsContext("delete images").Seed();

            await using var context = Deps.FoodStuffsContext("delete images");
            var data = context.FoodStuffsData();

            var recipeToDelete = context.Recipe
                .Include(r => r.Image)
                .ThenInclude(r => r.Blob)
                .AsNoTracking()
                .First(r => r.Name == "Recipe1");

            Assert.True(recipeToDelete.Image.Any());
            Assert.True(recipeToDelete.Image.Select(i => i.Blob).Any());
            Assert.Equal(recipeToDelete.Image.Count, recipeToDelete.Image.Select(i => i.Blob).Count());

            var result = await new DeleteRecipe.Handler(data)
                .Handle(new DeleteRecipe.Request(recipeToDelete.Id));

            Assert.True(result.IsSuccess);

            Assert.Equal(recipeToDelete.Id, result.Value.Id);

            var imageIds = recipeToDelete.Image.Select(i => i.Id);

            Assert.False(context.Image.Any(i => imageIds.Contains(i.Id)));
            Assert.False(context.Blob.Any(b => imageIds.Contains(b.Id)));
        }

        [Fact]
        public async Task DeleteRecipe_returns_failure_when_recipe_does_not_exist()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var result = await new DeleteRecipe.Handler(data)
                .Handle(new DeleteRecipe.Request(-22));

            Assert.True(result.IsFailed);
        }

        [Fact]
        public async Task SaveRecipe_creates_new_recipe_when_id_0_is_specified()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var result = await new SaveRecipe.Handler(data)
                .Handle(new SaveRecipe.Request(0, "New", "New", "New", null, 20, new[] { "Category2", "Category3", "Category4" }));

            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Id > 0);

            var maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesAndImagesSpecification(result.Value.Id), default);

            Assert.True(maybeRecipe.HasValue);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, maybeRecipe.Value.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, maybeRecipe.Value.ModifiedOn);
            Assert.DoesNotContain("Category1", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category2", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category3", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category4", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
        }

        [Fact]
        public async Task SaveRecipe_updates_existing_recipe_when_exists()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var existingRecipeId = (await data.Recipes.ListAll(default)).First().Id;

            var result = await new SaveRecipe.Handler(data)
                .Handle(new SaveRecipe.Request(existingRecipeId, "New", "New", "New", null, 20, new[] { "Category2", "Category3", "Category4" }));

            Assert.True(result.IsSuccess);
            Assert.Equal(existingRecipeId, result.Value.Id);

            var updatedRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesAndImagesSpecification(existingRecipeId), default);
            Assert.True(updatedRecipe.HasValue);
            Assert.Equal(Deps.DateTimeServiceEarly.Moment, updatedRecipe.Value.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, updatedRecipe.Value.ModifiedOn);
            Assert.DoesNotContain("Category1", updatedRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category2", updatedRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category3", updatedRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category4", updatedRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
        }
    }
}
