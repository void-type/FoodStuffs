using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Domain.Recipes;
using FoodStuffs.Web.Users;
using System.Linq;
using VoidCore.Domain;
using VoidCore.Model.Data;
using Xunit;

namespace FoodStuffs.Test.Model
{
    public class RecipeEventTests
    {
        [Fact]
        public async void GetRecipeFound()
        {
            using(var data = MockFactory.FoodStuffsData())
            {
                MockFactory.PopulateWithData(data);

                var result = await new GetRecipe.Handler(data)
                    .Handle(new GetRecipe.Request(11));

                Assert.True(result.IsSuccess);
                Assert.Equal(11, result.Value.Id);
            }
        }

        [Fact]
        public async void GetRecipeNotFound()
        {
            using(var data = MockFactory.FoodStuffsData())
            {
                MockFactory.PopulateWithData(data);

                var result = await new GetRecipe.Handler(data)
                    .Handle(new GetRecipe.Request(1000));

                Assert.True(result.IsFailed);
            }
        }

        [Fact]
        public async void ListRecipes()
        {
            using(var data = MockFactory.FoodStuffsData())
            {
                MockFactory.PopulateWithData(data);

                var result = await new ListRecipes.Handler(data)
                    .Handle(new ListRecipes.Request(1, 2, null, null, null));

                Assert.True(result.IsSuccess);
                Assert.Equal(2, result.Value.Count);
                Assert.Equal(3, result.Value.TotalCount);
            }
        }

        [Fact]
        public async void ListRecipesWithSearch()
        {
            using(var data = MockFactory.FoodStuffsData())
            {
                MockFactory.PopulateWithData(data);

                var result = await new ListRecipes.Handler(data)
                    .Handle(new ListRecipes.Request(1, 2, "1", null, null));

                Assert.True(result.IsSuccess);
                Assert.Equal(1, result.Value.Count);
                Assert.Equal(1, result.Value.TotalCount);
            }
        }

        [Fact]
        public async void ListRecipesNoneFound()
        {
            using(var data = MockFactory.FoodStuffsData())
            {
                MockFactory.PopulateWithData(data);

                var result = await new ListRecipes.Handler(data)
                    .Handle(new ListRecipes.Request(1, 2, "nothing matches", null, null));

                Assert.True(result.IsSuccess);
                Assert.Equal(0, result.Value.Count);
                Assert.Equal(0, result.Value.TotalCount);
            }
        }

        [Fact]
        public async void DeleteRecipeFound()
        {
            using(var data = MockFactory.FoodStuffsData())
            {
                MockFactory.PopulateWithData(data);

                var result = await new DeleteRecipe.Handler(data)
                    .Handle(new DeleteRecipe.Request(11));

                Maybe<Recipe> maybeRecipe = data.Recipes.Stored.FirstOrDefault(r => r.Id == 11);

                Assert.True(result.IsSuccess);
                Assert.True(maybeRecipe.HasNoValue);
                Assert.Equal(11, result.Value.Id);
            }
        }

        [Fact]
        public async void DeleteRecipeNotFound()
        {
            using(var data = MockFactory.FoodStuffsData())
            {
                MockFactory.PopulateWithData(data);

                var result = await new DeleteRecipe.Handler(data)
                    .Handle(new DeleteRecipe.Request(1000));

                Maybe<Recipe> maybeRecipe = data.Recipes.Stored.FirstOrDefault(r => r.Id == 1000);

                Assert.True(result.IsFailed);
                Assert.True(maybeRecipe.HasNoValue);
            }
        }

        [Fact]
        public async void SaveExistingRecipe()
        {
            using(var data = MockFactory.FoodStuffsData())
            {
                MockFactory.PopulateWithData(data);

                var result = await new SaveRecipe.Handler(data, new AuditUpdater(MockFactory.DateTimeServiceEarly, new SingleUserAccessor()))
                    .Handle(new SaveRecipe.Request(11, "New", "New", "New", null, 20, new string[] { "Category3" }));

                Maybe<Recipe> maybeRecipe = data.Recipes.Stored.FirstOrDefault(r => r.Id == 11);

                Assert.True(result.IsSuccess);
                Assert.Equal(11, result.Value.Id);
                Assert.True(maybeRecipe.HasValue);
            }
        }

        [Fact]
        public async void SaveNewRecipe()
        {
            using(var data = MockFactory.FoodStuffsData())
            {
                MockFactory.PopulateWithData(data);

                var result = await new SaveRecipe.Handler(data, new AuditUpdater(MockFactory.DateTimeServiceEarly, new SingleUserAccessor()))
                    .Handle(new SaveRecipe.Request(0, "New", "New", "New", null, 20, new string[] { "Category3" }));

                Maybe<Recipe> maybeRecipe = data.Recipes.Stored.FirstOrDefault(r => r.Id == result.Value.Id);

                Assert.True(result.IsSuccess);
                Assert.True(result.Value.Id > 0);
                Assert.True(maybeRecipe.HasValue);
            }
        }
    }
}
