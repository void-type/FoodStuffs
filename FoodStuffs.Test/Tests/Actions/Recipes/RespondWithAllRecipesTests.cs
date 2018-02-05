using Core.Model.Actions.Chain;
using FoodStuffs.Data.Models;
using FoodStuffs.Data.Services;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Test.Mocks;
using Xunit;

namespace FoodStuffs.Test.Tests.Actions.Recipes
{
    public class RespondWithAllRecipesTests
    {
        [Fact]
        public void RespondWithAllRecipesEmpty()
        {
            var responder = MockFactory.GetResponder;

            using (var data = new FoodStuffsEfMemoryData())
            {
                data.Users.Add(MockFactory.User1);
                new ActionChain(responder)
                    .Execute(new RespondWithRecipes(data));

                Assert.True(responder.ResponseCreated);
                Assert.Empty(responder.Response.DataList);
            }
        }

        [Fact]
        public void RespondWithAllRecipesNotEmpty()
        {
            var responder = MockFactory.GetResponder;

            using (var data = new FoodStuffsEfMemoryData())
            {
                data.Users.Add(MockFactory.User1);
                data.Recipes.Add(MockFactory.Recipe1);
                data.Recipes.Add(MockFactory.Recipe2);

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new RespondWithRecipes(data));

                Assert.True(responder.ResponseCreated);
                Assert.Equal(2, responder.Response.DataList.Count);
            }
        }

        [Theory]
        [InlineData("cat", 3)]
        [InlineData("1", 1)]
        [InlineData("1 2", 2)]
        [InlineData(null, 3)]
        [InlineData("cat 1 3", 2)]
        public void RespondWithNameSearchRecipesByCategory(string categorySearch, int expectedFound)
        {
            var responder = MockFactory.GetResponder;

            using (var data = new FoodStuffsEfMemoryData())
            {
                var recipe1 = MockFactory.Recipe1;
                var recipe2 = MockFactory.Recipe2;
                var recipe3 = MockFactory.Recipe3;

                var category1 = MockFactory.Category1;
                var category2 = MockFactory.Category2;
                var category3 = MockFactory.Category3;

                data.Users.Add(MockFactory.User1);

                data.Recipes.Add(MockFactory.Recipe1);
                data.Recipes.Add(MockFactory.Recipe2);
                data.Recipes.Add(MockFactory.Recipe3);

                data.Categories.Add(MockFactory.Category1);
                data.Categories.Add(MockFactory.Category2);
                data.Categories.Add(MockFactory.Category3);

                ICategoryRecipe categoryRecipe1 = new CategoryRecipe();
                categoryRecipe1.CategoryId = 1;
                categoryRecipe1.RecipeId = 1;
                categoryRecipe1.Recipe = recipe1;
                categoryRecipe1.Category = category1;

                ICategoryRecipe categoryRecipe2 = new CategoryRecipe();
                categoryRecipe2.CategoryId = 2;
                categoryRecipe2.RecipeId = 2;
                categoryRecipe2.Recipe = recipe2;
                categoryRecipe2.Category = category2;

                ICategoryRecipe categoryRecipe3 = new CategoryRecipe();
                categoryRecipe3.CategoryId = 3;
                categoryRecipe3.RecipeId = 3;
                categoryRecipe3.Recipe = recipe3;
                categoryRecipe3.Category = category3;

                recipe1.CategoryRecipes.Add(categoryRecipe1);
                recipe2.CategoryRecipes.Add(categoryRecipe2);
                recipe3.CategoryRecipes.Add(categoryRecipe3);

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new RespondWithRecipes(data, null, categorySearch));

                Assert.True(responder.ResponseCreated);
                Assert.Equal(expectedFound, responder.Response.DataList.Count);
            }
        }

        [Theory]
        [InlineData("recipe", 3)]
        [InlineData("cipe", 3)]
        [InlineData("", 3)]
        [InlineData(null, 3)]
        [InlineData("1", 1)]
        public void RespondWithNameSearchRecipesByName(string nameSearch, int expectedFound)
        {
            var responder = MockFactory.GetResponder;

            using (var data = new FoodStuffsEfMemoryData())
            {
                data.Users.Add(MockFactory.User1);
                data.Recipes.Add(MockFactory.Recipe1);
                data.Recipes.Add(MockFactory.Recipe2);
                data.Recipes.Add(MockFactory.Recipe3);

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new RespondWithRecipes(data, nameSearch));

                Assert.True(responder.ResponseCreated);
                Assert.Equal(expectedFound, responder.Response.DataList.Count);
            }
        }

        [Fact]
        public void RespondWithPaginatedRecipes()
        {
            var responder = MockFactory.GetResponder;

            using (var data = new FoodStuffsEfMemoryData())
            {
                data.Users.Add(MockFactory.User1);
                data.Recipes.Add(MockFactory.Recipe1);
                data.Recipes.Add(MockFactory.Recipe2);
                data.Recipes.Add(MockFactory.Recipe3);

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new RespondWithRecipes(data, null, null, 2, 2));

                Assert.True(responder.ResponseCreated);
                Assert.Single(responder.Response.DataList);
            }
        }
    }
}