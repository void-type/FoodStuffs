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
            var responder = MockFactory.Responder;

            using (var data = TestDataFactory.FoodStuffsDb())
            {
                new ActionChain(responder)
                    .Execute(new RespondWithAllRecipes(data));
            }

            Assert.Empty(responder.Response.DataList);
        }

        [Fact]
        public void RespondWithAllRecipesNotEmpty()
        {
            var responder = MockFactory.Responder;

            using (var data = TestDataFactory.FoodStuffsDb("a"))
            {
                data.Recipes.Add(new Recipe
                {
                    Id = 1,
                    Name = "Alfredo"
                });

                data.SaveChanges();
            }

            using (var data = TestDataFactory.FoodStuffsDb("a"))
            {
                new ActionChain(responder)
                    .Execute(new RespondWithAllRecipes(data));
            }

            Assert.NotEmpty(responder.Response.DataList);
        }
    }
}