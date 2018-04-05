using Core.Model.Actions.Chain;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.ViewModels;
using FoodStuffs.Test.Mocks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FoodStuffs.Test.Tests.Actions.Recipes
{
    public class CreateRecipeTests
    {
        [Fact]
        public void CreateRecipe()
        {
            var now = MockFactory.DateTimeServiceEarly;
            var responder = MockFactory.Responder;

            var newRecipeViewModel = new RecipeViewModel
            {
                Name = "Recipe1",
                CookTimeMinutes = 2,
                PrepTimeMinutes = 2,
                Categories = new List<string>
                {
                    "Category1",
                    "Category2"
                }
            };

            using (var data = MockFactory.FoodStuffsData())
            {
                data.Users.Add(MockFactory.User1);

                var categoryToAdd = MockFactory.Category2;

                data.Categories.Add(categoryToAdd);

                data.SaveChanges();

                new ActionChain(responder)
                    .Execute(new CreateRecipe(data, now, newRecipeViewModel, 11));

                Assert.True(responder.ResponseCreated);

                Assert.Equal(1, data.Recipes.Stored.Count());
                Assert.Equal(2, data.Categories.Stored.Count());
                Assert.Equal(2, data.CategoryRecipes.Stored.Count());

                Assert.NotNull(data.Categories.Stored.GetByName("Category1"));
                Assert.NotNull(data.Categories.Stored.GetByName("Category2"));
            }
        }
    }
}