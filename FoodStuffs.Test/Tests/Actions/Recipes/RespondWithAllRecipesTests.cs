using FoodStuffs.Data.FoodStuffsDb;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Actions.Core.Chain;
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
            using (var data = new FoodStuffsMemoryData("RespondWithAllRecipesEmpty"))
            {
                var responder = MockFactory.Responder;

                new ActionChain(responder)
                    .Execute(new RespondWithAllRecipes(data));

                Assert.Empty(responder.Response.DataList);
                Assert.True(responder.ResponseCreated);
            }
        }

        [Fact]
        public void RespondWithAllRecipesNotEmpty()
        {
            using (var data = new FoodStuffsMemoryData("RespondWithAllRecipesNotEmpty"))
            {
                data.Recipes.Add(new Recipe
                {
                    Id = 1,
                    Name = "Alfredo"
                });

                data.SaveChanges();
            }

            using (var data = new FoodStuffsMemoryData("RespondWithAllRecipesNotEmpty"))
            {
                var responder = MockFactory.Responder;

                new ActionChain(responder)
                    .Execute(new RespondWithAllRecipes(data));

                Assert.NotEmpty(responder.Response.DataList);
                Assert.True(responder.ResponseCreated);
            }
        }
    }
}