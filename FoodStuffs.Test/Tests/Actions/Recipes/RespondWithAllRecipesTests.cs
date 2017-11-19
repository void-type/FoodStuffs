using Core.Model.Actions.Chain;
using FoodStuffs.Data.FoodStuffsDb.Models;
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
            using (var data = new FoodStuffsMemoryData())
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
            using (var data = new FoodStuffsMemoryData())
            {
                data.Recipes.Add(new Recipe
                {
                    Id = 1,
                    Name = "Alfredo"
                });

                data.SaveChanges();

                var responder = MockFactory.Responder;

                new ActionChain(responder)
                    .Execute(new RespondWithAllRecipes(data));

                Assert.NotEmpty(responder.Response.DataList);
                Assert.True(responder.ResponseCreated);
            }
        }
    }
}