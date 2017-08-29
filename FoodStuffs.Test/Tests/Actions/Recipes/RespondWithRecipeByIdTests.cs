using FoodStuffs.Data.FoodStuffsDb;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Actions.Core.Chain;
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

            using (var data = new FoodStuffsMemoryData("b"))
            {
                data.Recipes.Add(new Recipe
                {
                    Id = 1,
                    Name = "Alfredo"
                });

                data.Recipes.Add(new Recipe
                {
                    Id = 2,
                    Name = "Alfredo"
                });

                data.Recipes.Add(new Recipe
                {
                    Id = 3,
                    Name = "Alfredo"
                });

                data.SaveChanges();
            }

            using (var data = new FoodStuffsMemoryData("b"))
            {
                new ActionChain(responder)
                    .Execute(new RespondWithRecipeById(data, 2));
            }

            Assert.NotNull(responder.Response.DataItem);
        }

        [Fact]
        public void RespondWithRecipeByIdNotFound()
        {
            var responder = MockFactory.Responder;

            using (var data = new FoodStuffsMemoryData("b"))
            {
                new ActionChain(responder)
                    .Execute(new RespondWithRecipeById(data, 5));
            }

            Assert.Null(responder.Response.DataItem);
        }
    }
}