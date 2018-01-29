using Core.Model.Actions.Chain;
using FoodStuffs.Data.Services;
using FoodStuffs.Model.Actions.Recipes;
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
        [InlineData("recipe", 3)]
        [InlineData("cipe", 3)]
        [InlineData("", 3)]
        [InlineData(null, 3)]
        [InlineData("1", 1)]
        public void RespondWithNameSearchRecipes(string nameSearch, int expectedFound)
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