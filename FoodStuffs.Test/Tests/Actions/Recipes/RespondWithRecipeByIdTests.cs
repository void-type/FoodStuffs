using Core.Model.Actions.Chain;
using FoodStuffs.Data.Services;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Test.Mocks;
using Xunit;

namespace FoodStuffs.Test.Tests.Actions.Recipes
{
    public class RespondWithRecipeByIdTests
    {
        [Fact]
        public void RespondWithRecipeByIdFound()
        {
            var responder = MockFactory.Responder;

            using (var data = new FoodStuffsEfMemoryData())
            {
                data.Users.Add(MockFactory.User1);

                data.Recipes.Add(MockFactory.Recipe1);
                data.Recipes.Add(MockFactory.Recipe2);
                data.Recipes.Add(MockFactory.Recipe3);

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new RespondWithRecipeById(data, 2));

                Assert.True(responder.ResponseCreated);
                Assert.NotNull(responder.Response.DataItem);
            }
        }

        [Fact]
        public void RespondWithRecipeByIdNotFound()
        {
            using (var data = new FoodStuffsEfMemoryData())
            {
                var responder = MockFactory.Responder;

                new ActionChain(responder)
                    .Execute(new RespondWithRecipeById(data, 5));

                Assert.True(responder.ResponseCreated);
                Assert.Null(responder.Response.DataItem);
            }
        }
    }
}