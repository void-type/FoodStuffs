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
        public void Delete()
        {
            var data = new FoodStuffsMemoryData();
            var responder = MockFactory.Responder;

            data.Recipes.Add(new Recipe
            {
                Id = 1,
                Name = "New Recipe1",
                CookTimeMinutes = 2,
                PrepTimeMinutes = 2
            });

            data.Recipes.Add(new Recipe
            {
                Id = 2,
                Name = "New Recipe2",
                CookTimeMinutes = 2,
                PrepTimeMinutes = 2
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

            new ActionChain(responder)
                .Execute(new DeleteRecipe(data, 1))
                .Execute(new SaveChangesToData(data));

            Assert.Equal(1, data.Recipes.Stored.Count());
            Assert.Equal(1, data.Categories.Stored.Count());
            Assert.Equal(1, data.CategoryRecipes.Stored.Count());
            Assert.False(responder.ResponseCreated);
        }
    }
}