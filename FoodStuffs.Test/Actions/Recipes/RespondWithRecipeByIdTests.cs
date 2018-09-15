using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Test.Mocks;
using VoidCore.Model.Actions.Chain;
using Xunit;

namespace FoodStuffs.Test.Actions.Recipes
{
    public class RespondWithRecipeByIdTests
    {
        [Fact]
        public void RespondWithRecipeByIdFound()
        {
            var responder = MockFactory.Responder;

            using(var data = MockFactory.FoodStuffsData())
            {
                data.Users.Add(MockFactory.User1);

                data.Recipes.Add(MockFactory.Recipe1);
                data.Recipes.Add(MockFactory.Recipe2);
                data.Recipes.Add(MockFactory.Recipe3);

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new RespondWithRecipeById(data, 12));

                Assert.True(responder.ResponseCreated);
                Assert.NotNull(responder.Response.Item);
            }
        }

        [Fact]
        public void RespondWithRecipeByIdNotFound()
        {
            using(var data = MockFactory.FoodStuffsData())
            {
                var responder = MockFactory.Responder;

                new ActionChain(responder)
                    .Execute(new RespondWithRecipeById(data, 15));

                Assert.True(responder.ResponseCreated);
                Assert.Null(responder.Response.Item);
            }
        }
    }
}
