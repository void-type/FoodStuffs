using Core.Model.Actions.Chain;
using FoodStuffs.Data.Models;
using FoodStuffs.Data.Services;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Queries;
using FoodStuffs.Test.Mocks;
using System.Linq;
using Xunit;

namespace FoodStuffs.Test.Tests.Actions.Recipes
{
    public class DeleteRecipeTests
    {
        [Fact]
        public void DeleteRecipe()
        {
            var responder = MockFactory.GetResponder;

            using (var data = new FoodStuffsEfMemoryData())
            {
                data.Users.Add(MockFactory.User1);

                data.Recipes.Add(MockFactory.Recipe1);

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 1, 1));

                Assert.True(responder.ResponseCreated);

                Assert.Equal(0, data.Recipes.Stored.Count());
                Assert.Equal(0, data.Categories.Stored.Count());
                Assert.Equal(0, data.CategoryRecipes.Stored.Count());
            }
        }

        [Fact]
        public void DeleteRecipeAndRelations()
        {
            var responder = MockFactory.GetResponder;

            using (var data = new FoodStuffsEfMemoryData())
            {
                data.Users.Add(MockFactory.User1);

                data.Recipes.Add(MockFactory.Recipe1);

                data.Categories.Add(MockFactory.Category1);

                data.Categories.Add(MockFactory.Category2);

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    RecipeId = 1,
                    CategoryId = 1
                });

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    RecipeId = 1,
                    CategoryId = 2
                });

                data.Recipes.Add(MockFactory.Recipe2);

                data.Categories.Add(MockFactory.Category3);

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    RecipeId = 2,
                    CategoryId = 2
                });

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    RecipeId = 2,
                    CategoryId = 3
                });

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 2, 1));

                Assert.True(responder.ResponseCreated);

                Assert.Equal(1, data.Recipes.Stored.Count());
                Assert.Equal(2, data.Categories.Stored.Count());
                Assert.Equal(2, data.CategoryRecipes.Stored.Count());

                Assert.NotNull(data.Recipes.Stored.GetById(1));
                Assert.NotNull(data.Categories.Stored.GetById(1));
                Assert.NotNull(data.Categories.Stored.GetById(2));
                Assert.NotNull(data.CategoryRecipes.Stored.GetById(1, 1));
                Assert.NotNull(data.CategoryRecipes.Stored.GetById(1, 2));

                Assert.Null(data.Recipes.Stored.GetById(2));
                Assert.Null(data.Categories.Stored.GetById(3));
                Assert.Null(data.CategoryRecipes.Stored.GetById(2, 2));
                Assert.Null(data.CategoryRecipes.Stored.GetById(2, 3));
            }
        }

        [Fact]
        public void DeleteRecipeNotFound()
        {
            var responder = MockFactory.GetResponder;

            using (var data = new FoodStuffsEfMemoryData())
            {
                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 2, 1));
            }

            Assert.True(responder.ResponseCreated);
            Assert.Single(responder.ValidationErrors);
        }
    }
}