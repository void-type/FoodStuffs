using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Domain.Recipes;
using FoodStuffs.Model.Queries;
using FoodStuffs.Web.Auth;
using System.Linq;
using VoidCore.Domain;
using VoidCore.Model.Data;
using Xunit;

namespace FoodStuffs.Test.Model.Domain
{

    public class RecipeEventTests
    {
        [Fact]
        public async void GetRecipeFound()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            var result = await new GetRecipe.Handler(data)
                .Handle(new GetRecipe.Request(11));

            Assert.True(result.IsSuccess);
            Assert.Equal(11, result.Value.Id);
            Assert.Equal("Recipe1", result.Value.Name);
            Assert.Equal("ing", result.Value.Ingredients);
            Assert.Equal("dir", result.Value.Directions);
            Assert.Equal(21, result.Value.CookTimeMinutes);
            Assert.Equal(2, result.Value.PrepTimeMinutes);
            Assert.Equal("11", result.Value.CreatedBy);
            Assert.Equal(MockFactory.DateTimeServiceEarly.Moment, result.Value.CreatedOn);
            Assert.Equal("12", result.Value.ModifiedBy);
            Assert.Equal(MockFactory.DateTimeServiceLate.Moment, result.Value.ModifiedOn);
            Assert.Equal(2, result.Value.Categories.Count());
        }

        [Fact]
        public async void GetRecipeNotFound()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            var result = await new GetRecipe.Handler(data)
                .Handle(new GetRecipe.Request(1000));

            Assert.True(result.IsFailed);
        }

        [Fact]
        public async void ListRecipes()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(2, 1, null, null, null));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(3, result.Value.TotalCount);
            Assert.Equal(2, result.Value.Page);
            Assert.Equal(1, result.Value.Take);
            Assert.Contains("Recipe2", result.Value.Items.Select(r => r.Name));
            Assert.Contains(12, result.Value.Items.Select(r => r.Id));
            Assert.Contains("Category1", result.Value.Items.SelectMany(r => r.Categories));
        }

        [Fact]
        public async void ListRecipesSortDesc()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(1, 1, null, null, "descending"));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(3, result.Value.TotalCount);
            Assert.Equal(1, result.Value.Page);
            Assert.Equal(1, result.Value.Take);
            Assert.Contains("Recipe3", result.Value.Items.Select(r => r.Name));
        }

        [Fact]
        public async void ListRecipesSortAscend()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            await data.Recipes.Add(new Recipe() { Name = "ANewRecipe" });

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(1, 1, null, null, "ascending"));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(4, result.Value.TotalCount);
            Assert.Equal(1, result.Value.Page);
            Assert.Equal(1, result.Value.Take);
            Assert.Contains("ANewRecipe", result.Value.Items.Select(r => r.Name));
        }

        [Fact]
        public async void ListRecipesNameSearch()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(1, 2, "recipe 2", null, null));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(1, result.Value.TotalCount);
            Assert.Equal(1, result.Value.Page);
            Assert.Equal(2, result.Value.Take);
            Assert.Contains("Recipe2", result.Value.Items.Select(r => r.Name));
        }

        [Fact]
        public async void ListRecipesCategorySearch()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(1, 4, null, "cat 1 2", null));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(1, result.Value.TotalCount);
            Assert.Equal(1, result.Value.Page);
            Assert.Equal(4, result.Value.Take);
            Assert.Contains("Recipe1", result.Value.Items.Select(r => r.Name));
            Assert.DoesNotContain("Recipe2", result.Value.Items.Select(r => r.Name));
            Assert.DoesNotContain("Recipe3", result.Value.Items.Select(r => r.Name));
        }

        [Fact]
        public async void ListRecipesNoneFoundByNameSearch()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(1, 2, "nothing matches", null, null));

            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value.Count);
            Assert.Equal(0, result.Value.TotalCount);
        }

        [Fact]
        public async void ListRecipesNoneFoundByCategorySearch()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(1, 2, null, "nothing matches", null));

            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value.Count);
            Assert.Equal(0, result.Value.TotalCount);
        }

        [Fact]
        public async void DeleteRecipeFound()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            var result = await new DeleteRecipe.Handler(data)
                .Handle(new DeleteRecipe.Request(11));

            Maybe<Recipe> maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(11));

            Assert.True(result.IsSuccess);
            Assert.True(maybeRecipe.HasNoValue);
            Assert.Equal(11, result.Value.Id);
        }

        [Fact]
        public async void DeleteRecipeNotFound()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            var result = await new DeleteRecipe.Handler(data)
                .Handle(new DeleteRecipe.Request(1000));

            Maybe<Recipe> maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(1000));

            Assert.True(result.IsFailed);
            Assert.True(maybeRecipe.HasNoValue);
        }

        [Fact]
        public async void SaveNewRecipe()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            var result = await new SaveRecipe.Handler(data, new AuditUpdater(MockFactory.DateTimeServiceEarly, new SingleUserAccessor()))
                .Handle(new SaveRecipe.Request(0, "New", "New", "New", null, 20, new [] { "Category2", "Category3", "Category4" }));

            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Id > 0);

            Maybe<Recipe> maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(result.Value.Id));
            Assert.True(maybeRecipe.HasValue);
            Assert.Equal(MockFactory.DateTimeServiceEarly.Moment, maybeRecipe.Value.CreatedOn);
            Assert.Equal(MockFactory.DateTimeServiceEarly.Moment, maybeRecipe.Value.ModifiedOn);
            Assert.DoesNotContain("Category1", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category2", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category3", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category4", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));

            maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(12));
            Assert.True(maybeRecipe.HasValue);
            Assert.Contains("Category1", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.DoesNotContain("Category2", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.DoesNotContain("Category3", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.DoesNotContain("Category4", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
        }

        [Fact]
        public async void SaveExistingRecipe()
        {
            var data = MockFactory.FoodStuffsData();
            MockFactory.PopulateWithData(data);

            Maybe<Recipe> maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(11));
            Assert.True(maybeRecipe.HasValue);

            var result = await new SaveRecipe.Handler(data, new AuditUpdater(MockFactory.DateTimeServiceEarly, new SingleUserAccessor()))
                .Handle(new SaveRecipe.Request(11, "New", "New", "New", null, 20, new [] { "Category2", "Category3", "Category4" }));

            Assert.True(result.IsSuccess);
            Assert.Equal(11, result.Value.Id);

            maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(11));
            Assert.True(maybeRecipe.HasValue);
            Assert.Equal(MockFactory.DateTimeServiceEarly.Moment, maybeRecipe.Value.CreatedOn);
            Assert.Equal(MockFactory.DateTimeServiceEarly.Moment, maybeRecipe.Value.ModifiedOn);
            Assert.DoesNotContain("Category1", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category2", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category3", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category4", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));

            maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(12));
            Assert.True(maybeRecipe.HasValue);
            Assert.Contains("Category1", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.DoesNotContain("Category2", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.DoesNotContain("Category3", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.DoesNotContain("Category4", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
        }
    }
}
