using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using FoodStuffs.Test.Mocks;
using System.Linq;
using VoidCore.Model.Actions.Chain;
using Xunit;

namespace FoodStuffs.Test.Actions.Recipes
{
    public class DeleteRecipeTests
    {
        [Fact]
        public void DeleteRecipe()
        {
            var responder = MockFactory.Responder;

            using(var data = MockFactory.FoodStuffsData())
            {
                data.Users.Add(MockFactory.User1);

                data.Recipes.Add(MockFactory.Recipe1);

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 11));

                Assert.True(responder.ResponseCreated);

                Assert.Equal(0, data.Recipes.Stored.Count());
                Assert.Equal(0, data.Categories.Stored.Count());
                Assert.Equal(0, data.CategoryRecipes.Stored.Count());
            }
        }

        [Fact]
        public void DeleteRecipeAndRelations()
        {
            var responder = MockFactory.Responder;

            using(var data = MockFactory.FoodStuffsData())
            {
                data.Users.Add(MockFactory.User1);

                data.Recipes.Add(MockFactory.Recipe1);

                data.Categories.Add(MockFactory.Category1);

                data.Categories.Add(MockFactory.Category2);

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    RecipeId = 11,
                        CategoryId = 11
                });

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    RecipeId = 11,
                        CategoryId = 12
                });

                data.Recipes.Add(MockFactory.Recipe2);

                data.Categories.Add(MockFactory.Category3);

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    RecipeId = 12,
                        CategoryId = 12
                });

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    RecipeId = 12,
                        CategoryId = 13
                });

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 12));

                Assert.True(responder.ResponseCreated);

                Assert.Equal(1, data.Recipes.Stored.Count());
                Assert.Equal(2, data.Categories.Stored.Count());
                Assert.Equal(2, data.CategoryRecipes.Stored.Count());

                Assert.NotNull(data.Recipes.Stored.GetById(11));
                Assert.NotNull(data.Categories.Stored.GetById(11));
                Assert.NotNull(data.Categories.Stored.GetById(12));
                Assert.NotNull(data.CategoryRecipes.Stored.GetById(11, 11));
                Assert.NotNull(data.CategoryRecipes.Stored.GetById(11, 12));

                Assert.Null(data.Recipes.Stored.GetById(12));
                Assert.Null(data.Categories.Stored.GetById(13));
                Assert.Null(data.CategoryRecipes.Stored.GetById(12, 12));
                Assert.Null(data.CategoryRecipes.Stored.GetById(12, 13));
            }
        }

        [Fact]
        public void DeleteRecipeNotFound()
        {
            var responder = MockFactory.Responder;

            using(var data = MockFactory.FoodStuffsData())
            {
                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 12));
            }

            Assert.True(responder.ResponseCreated);
            Assert.Single(responder.ValidationErrors);
        }
    }
}
