using Core.Model.Actions.Chain;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Data.Test;
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
            using (var data = new FoodStuffsMemoryData())
            {
                data.Recipes.Add(new Recipe
                {
                    Id = 1,
                    Name = "Recipe1"
                });

                data.SaveChanges();

                var responder = MockFactory.Responder;

                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 1));
                Assert.False(responder.ResponseCreated);

                Assert.Equal(0, data.Recipes.Stored.Count());
                Assert.Equal(0, data.Categories.Stored.Count());
                Assert.Equal(0, data.CategoryRecipes.Stored.Count());
            }
        }

        [Fact]
        public void DeleteRecipeAndRelations()
        {
            using (var data = new FoodStuffsMemoryData())
            {
                data.Recipes.Add(new Recipe
                {
                    Id = 1,
                    Name = "Recipe1"
                });

                data.Categories.Add(new Category
                {
                    Id = 1,
                    Name = "Category1"
                });

                data.Categories.Add(new Category
                {
                    Id = 2,
                    Name = "Category2"
                });

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

                data.Recipes.Add(new Recipe
                {
                    Id = 2,
                    Name = "Recipe2"
                });

                data.Categories.Add(new Category
                {
                    Id = 3,
                    Name = "Category3"
                });

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

                var responder = MockFactory.Responder;

                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 2));
                Assert.False(responder.ResponseCreated);

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
            var responder = MockFactory.Responder;

            using (var data = new FoodStuffsMemoryData())
            {
                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 2));
            }

            Assert.True(responder.ResponseCreated);
            Assert.Single(responder.ValidationErrors);
        }
    }
}