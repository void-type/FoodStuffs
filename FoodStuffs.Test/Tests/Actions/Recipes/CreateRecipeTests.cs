using FoodStuffs.Data.FoodStuffsDb;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Actions.Core.Chain;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Interfaces.Domain;
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
            var now = MockFactory.EarlyDateTimeService;
            var responder = MockFactory.Responder;

            var newRecipeViewModel = new RecipeViewModel
            {
                Id = 1,
                Name = "ChangedRecipeName 1",
                CookTimeMinutes = 2,
                PrepTimeMinutes = 2,
                Categories = new List<ICategory> {
                    new Category
                    {
                        Name = "New Category1"
                    },
                    new Category
                    {
                        Name = "New Category2"
                    }
                }
            };

            var user = new User
            {
                Id = 2
            };

            using (var data = new FoodStuffsMemoryData("CreateRecipe"))
            {
                data.Categories.Add(new Category
                {
                    Name = "New Category2"
                });

                data.SaveChanges();
            }

            using (var data = new FoodStuffsMemoryData("CreateRecipe"))
            {
                new ActionChain(responder)
                    .Execute(new CreateRecipe(data, now, user, newRecipeViewModel))
                    .Execute(new SaveChangesToData(data));

                Assert.Equal(1, data.Recipes.Stored.Count());
                Assert.Equal(2, data.Categories.Stored.Count());
                Assert.Equal(2, data.CategoryRecipes.Stored.Count());
                Assert.False(responder.ResponseCreated);
            }

            using (var data = new FoodStuffsMemoryData("CreateRecipe"))
            {
                new ActionChain(responder)
                    .Execute(new CreateRecipe(data, now, user, newRecipeViewModel))
                    .Execute(new SaveChangesToData(data));

                Assert.Equal(2, data.Recipes.Stored.Count());
                Assert.Equal(2, data.Categories.Stored.Count());
                Assert.Equal(4, data.CategoryRecipes.Stored.Count());
                Assert.False(responder.ResponseCreated);
            }
        }
    }
}