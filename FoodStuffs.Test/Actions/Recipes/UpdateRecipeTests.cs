using Core.Model.Actions.Chain;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.ViewModels;
using FoodStuffs.Test.Mocks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FoodStuffs.Test.Actions.Recipes
{
    public class UpdateRecipeTests
    {
        [Fact]
        public void UpdateRecipeAndRelations()
        {
            var responder = MockFactory.Responder;
            var then = MockFactory.DateTimeServiceEarly;
            var now = MockFactory.DateTimeServiceLate;

            using (var data = MockFactory.FoodStuffsData())
            {
                data.Users.Add(MockFactory.User1);
                data.Users.Add(MockFactory.User2);

                data.Categories.Add(MockFactory.Category1);
                data.Categories.Add(MockFactory.Category2);
                data.Categories.Add(MockFactory.Category3);

                var recipeToUpdate = MockFactory.Recipe1;

                data.Recipes.Add(recipeToUpdate);

                data.Recipes.Add(MockFactory.Recipe2);

                data.CategoryRecipes.AddRange(new List<CategoryRecipe> {
                    new CategoryRecipe
                    {
                        RecipeId = 11,
                        CategoryId = 11
                    },

                    new CategoryRecipe
                    {
                        RecipeId = 11,
                        CategoryId = 12
                    },

                    new CategoryRecipe
                    {
                        RecipeId = 11,
                        CategoryId = 13
                    },

                    new CategoryRecipe
                    {
                        RecipeId = 12,
                        CategoryId = 12
                    },

                    new CategoryRecipe
                    {
                        RecipeId = 12,
                        CategoryId = 13
                    }
                });

                data.SaveChanges();

                var newRecipeViewModel = new RecipeViewModel
                {
                    Id = recipeToUpdate.Id,
                    Name = "ChangedRecipeName 1",
                    CookTimeMinutes = 3,
                    PrepTimeMinutes = 4,
                    Categories = new List<string>
                    {
                        "Category3","Category4"
                    },
                };

                new ActionChain(responder)
                    .Execute(new UpdateRecipe(data, now, newRecipeViewModel, 12));

                var changedRecipe = data.Recipes.Stored.GetById(recipeToUpdate.Id);

                Assert.True(responder.ResponseCreated);

                Assert.Equal("ChangedRecipeName 1", changedRecipe.Name);
                Assert.Equal(4, changedRecipe.PrepTimeMinutes);
                Assert.Equal(then.Moment, changedRecipe.CreatedOnUtc);
                Assert.Equal(now.Moment, changedRecipe.ModifiedOnUtc);
                Assert.Equal(11, changedRecipe.CreatedByUserId);
                Assert.Equal(12, changedRecipe.ModifiedByUserId);

                var newCategory4 = data.Categories.Stored.Single(c => c.Name == "Category4");
                Assert.NotNull(newCategory4);

                Assert.Equal(2, data.Recipes.Stored.Count());
                Assert.Equal(3, data.Categories.Stored.Count());
                Assert.Equal(4, data.CategoryRecipes.Stored.Count());
            }
        }

        [Fact]
        public void UpdateRecipeNotFound()
        {
            var responder = MockFactory.Responder;
            var now = MockFactory.DateTimeServiceLate;

            var recipeViewModel = new RecipeViewModel
            {
                Id = 12,
                Categories = new List<string>
                {
                    "Category1"
                }
            };

            using (var data = MockFactory.FoodStuffsData())
            {
                new ActionChain(responder)
                    .Execute(new UpdateRecipe(data, now, recipeViewModel, 11));
            }

            Assert.True(responder.ResponseCreated);
            Assert.Single(responder.ValidationErrors);
        }
    }
}