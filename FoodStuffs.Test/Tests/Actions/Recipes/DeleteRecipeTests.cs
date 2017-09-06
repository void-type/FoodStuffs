using FoodStuffs.Data.FoodStuffsDb;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Actions.Core.Chain;
using FoodStuffs.Model.Actions.Core.Steps;
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
        public void DeleteRecipeAndRelations()
        {
            using (var data = new FoodStuffsMemoryData("DeleteRecipeAndRelations"))
            {
                data.Recipes.Add(new Recipe
                {
                    Id = 1,
                    Name = "Recipe1",
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
                    Name = "Recipe2",
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
            }

            var responder = MockFactory.Responder;

            using (var data = new FoodStuffsMemoryData("DeleteRecipeAndRelations"))
            {
                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 2))
                    .Execute(new SaveChangesToData(data));
            }

            using (var data = new FoodStuffsMemoryData("DeleteRecipeAndRelations"))
            {
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

                Assert.False(responder.ResponseCreated);
            }
        }

        [Fact]
        public void DeleteRecipe()
        {
            using (var data = new FoodStuffsMemoryData("DeleteRecipe"))
            {
                data.Recipes.Add(new Recipe
                {
                    Id = 1,
                    Name = "Recipe1",
                });

                data.SaveChanges();
            }

            var responder = MockFactory.Responder;

            using (var data = new FoodStuffsMemoryData("DeleteRecipe"))
            {
                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 1))
                    .Execute(new SaveChangesToData(data));
            }

            using (var data = new FoodStuffsMemoryData("DeleteRecipe"))
            {
                Assert.Equal(0, data.Recipes.Stored.Count());
                Assert.Equal(0, data.Categories.Stored.Count());
                Assert.Equal(0, data.CategoryRecipes.Stored.Count());

                Assert.False(responder.ResponseCreated);
            }
        }

        [Fact]
        public void DeleteRecipeNotFound()
        {
            var responder = MockFactory.Responder;

            using (var data = new FoodStuffsMemoryData("DeleteRecipeNotFound"))
            {
                new ActionChain(responder)
                    .Execute(new DeleteRecipe(data, 2))
                    .Execute(new SaveChangesToData(data));
            }

            Assert.True(responder.ResponseCreated);
            Assert.Equal(1, responder.ValidationErrors.Count);
        }
    }
}