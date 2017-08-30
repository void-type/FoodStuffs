using FoodStuffs.Data.FoodStuffsDb;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Actions.Core.Chain;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Actions.Recipes;
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
            using (var data = new FoodStuffsMemoryData("DeleteRecipe"))
            {
                data.Recipes.Add(new Recipe
                {
                    Id = 1,
                    Name = "New Recipe1",
                });

                data.Recipes.Add(new Recipe
                {
                    Id = 2,
                    Name = "New Recipe2",
                });

                data.Categories.Add(new Category
                {
                    Id = 1,
                    Name = "New Category1"
                });

                data.Categories.Add(new Category
                {
                    Id = 2,
                    Name = "New Category2"
                });

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    CategoryId = 1,
                    RecipeId = 1
                });

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    CategoryId = 2,
                    RecipeId = 2
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
                Assert.Equal(1, data.Recipes.Stored.Count());
                Assert.Equal(1, data.Categories.Stored.Count());
                Assert.Equal(1, data.CategoryRecipes.Stored.Count());
                Assert.False(responder.ResponseCreated);
            }
        }
    }
}