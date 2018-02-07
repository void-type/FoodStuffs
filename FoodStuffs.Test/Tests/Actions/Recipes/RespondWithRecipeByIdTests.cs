using Core.Model.Actions.Chain;
using FoodStuffs.Data.EntityFramework;
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
            var responder = MockFactory.GetResponder;

            using (var data = new FoodStuffsEfMemoryData())
            {
                data.Users.Add(MockFactory.User1);

                data.Recipes.Add(MockFactory.Recipe1);
                data.Recipes.Add(MockFactory.Recipe2);
                data.Recipes.Add(MockFactory.Recipe3);

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new RespondWithRecipeById(data, 12));

                Assert.True(responder.ResponseCreated);
                Assert.NotNull(responder.Response.DataItem);
            }
        }

        [Fact]
        public void RespondWithRecipeByIdNotFound()
        {
            using (var data = new FoodStuffsEfMemoryData())
            {
                var responder = MockFactory.GetResponder;

                new ActionChain(responder)
                    .Execute(new RespondWithRecipeById(data, 15));

                Assert.True(responder.ResponseCreated);
                Assert.Null(responder.Response.DataItem);
            }
        }
    }
}