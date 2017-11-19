using Core.Model.Actions.Chain;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Data.Test;
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
            using (var data = new FoodStuffsMemoryData())
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

                var responder = MockFactory.Responder;

                new ActionChain(responder)
                    .Execute(new RespondWithRecipeById(data, 2));

                Assert.NotNull(responder.Response.DataItem);
                Assert.True(responder.ResponseCreated);
            }
        }

        [Fact]
        public void RespondWithRecipeByIdNotFound()
        {
            using (var data = new FoodStuffsMemoryData())
            {
                var responder = MockFactory.Responder;

                new ActionChain(responder)
                    .Execute(new RespondWithRecipeById(data, 5));

                Assert.Null(responder.Response.DataItem);
                Assert.True(responder.ResponseCreated);
            }
        }
    }
}