using Core.Model.Actions.Chain;
using FoodStuffs.Data.Test;
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
            var responder = MockFactory.Responder;

            using (var data = new FoodStuffsMemoryData())
            {
                new ActionChain(responder)
                    .Execute(new RespondWithAllRecipes(data));

                Assert.True(responder.ResponseCreated);
                Assert.Empty(responder.Response.DataList);
            }
        }

        [Fact]
        public void RespondWithAllRecipesNotEmpty()
        {
            var responder = MockFactory.Responder;

            using (var data = new FoodStuffsMemoryData())
            {
                data.Users.Add(MockFactory.User1);

                data.Recipes.Add(MockFactory.Recipe1);
                data.Recipes.Add(MockFactory.Recipe2);

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new RespondWithAllRecipes(data));

                Assert.True(responder.ResponseCreated);
                Assert.Equal(2, responder.Response.DataList.Count);
            }
        }
    }
}