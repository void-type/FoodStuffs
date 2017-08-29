using FoodStuffs.Data.FoodStuffsDb;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Actions.Core.Chain;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.ViewModels;
using FoodStuffs.Test.Mocks;
using System.Linq;
using Xunit;

namespace FoodStuffs.Test.Tests.Actions.Recipes
{
    public class CreateRecipeTests
    {
        [Fact]
        public void CreateRecipe()
        {
            var newRecipeViewModel = new RecipeViewModel()
            {
                Name = "New Recipe",
                CookTimeMinutes = 2,
                PrepTimeMinutes = 2
            };

            newRecipeViewModel.Categories.Add(new Category
            {
                Name = "New Category1"
            });

            newRecipeViewModel.Categories.Add(new Category
            {
                Name = "New Category2"
            });

            var user = new User
            {
                Id = 2
            };

            var data = TestDataFactory.FoodStuffsDb();
            var now = MockFactory.EarlyDateTimeService;
            var responder = MockFactory.Responder;

            data.Categories.Add(new Category
            {
                Name = "New Category2"
            });

            data.SaveChanges();

            new ActionChain(responder)
                .Execute(new CreateRecipe(data, now, user, newRecipeViewModel))
                .Execute(new SaveChangesToData(data));

            Assert.Equal(1, data.Recipes.Stored.Count());
            Assert.Equal(2, data.Categories.Stored.Count());
            Assert.Equal(2, data.CategoryRecipes.Stored.Count());

            new ActionChain(responder)
                .Execute(new CreateRecipe(data, now, user, newRecipeViewModel))
                .Execute(new SaveChangesToData(data));

            Assert.Equal(2, data.Recipes.Stored.Count());
            Assert.Equal(2, data.Categories.Stored.Count());
            Assert.Equal(4, data.CategoryRecipes.Stored.Count());
        }
    }
}