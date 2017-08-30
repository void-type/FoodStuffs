using FoodStuffs.Data.FoodStuffsDb;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Actions.Core.Chain;
using FoodStuffs.Model.Actions.Core.Steps;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.ViewModels;
using FoodStuffs.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FoodStuffs.Test.Tests.Actions.Recipes
{
    public class UpdateRecipeTests
    {
        [Fact]
        public void UpdateRecipe()
        {
            var then = MockFactory.EarlyDateTimeService;
            var now = MockFactory.LateDateTimeService;

            var user = new User
            {
                Id = 1
            };

            using (var data = new FoodStuffsMemoryData("UpdateRecipe"))
            {
                data.Users.Add(user);
                data.Users.Add(new User
                {
                    Id = 2
                });

                data.Categories.Add(new Category
                {
                    Id = 2,
                    Name = "New Category2"
                });

                data.Categories.Add(new Category
                {
                    Id = 3,
                    Name = "New Category3"
                });

                data.Categories.Add(new Category
                {
                    Id = 4,
                    Name = "New Category4"
                });

                data.Recipes.Add(new Recipe
                {
                    Id = 1,
                    Name = "Recipe 1",
                    CookTimeMinutes = 2,
                    PrepTimeMinutes = 2,
                    CreatedOn = then.Now,
                    ModifiedOn = then.Now,
                    CreatedByUserId = 2
                });

                data.Recipes.Add(new Recipe
                {
                    Id = 2,
                    Name = "Recipe 2",
                    CookTimeMinutes = 2,
                    PrepTimeMinutes = 2
                });

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    CategoryId = 2,
                    RecipeId = 2
                });

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    CategoryId = 3,
                    RecipeId = 1
                });

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    CategoryId = 4,
                    RecipeId = 1
                });

                data.SaveChanges();
            }

            using (var data = new FoodStuffsMemoryData("UpdateRecipe"))
            {
                var responder = MockFactory.Responder;

                var newRecipeViewModel = new RecipeViewModel
                {
                    Id = 1,
                    Name = "ChangedRecipeName 1",
                    CookTimeMinutes = 3,
                    PrepTimeMinutes = 4,
                    Categories = new List<ICategory>
                    {
                        new Category
                        {
                            Name = "New Category1"
                        },
                        new Category
                        {
                            Name = "New Category2"
                        }
                    },
                    ModifiedOn = new DateTime(2222, 12, 12),
                    CreatedOn = new DateTime(1903, 12, 12),
                    ModifiedByUserId = 52,
                    CreatedByUserId = 53
                };

                new ActionChain(responder)
                    .Execute(new UpdateRecipe(data, now, user, newRecipeViewModel))
                    .Execute(new SaveChangesToData(data));

                var changedRecipe = data.Recipes.Stored.GetById(1);

                Assert.Equal("ChangedRecipeName 1", changedRecipe.Name);
                Assert.Equal(4, changedRecipe.PrepTimeMinutes);
                Assert.Equal(then.Now, changedRecipe.CreatedOn);
                Assert.Equal(now.Now, changedRecipe.ModifiedOn);
                Assert.Equal(2, changedRecipe.CreatedByUserId);
                Assert.Equal(1, changedRecipe.ModifiedByUserId);

                Assert.Equal(2, data.Recipes.Stored.Count());
                Assert.Equal(2, data.Categories.Stored.Count());
                Assert.Equal(3, data.CategoryRecipes.Stored.Count());
                Assert.False(responder.ResponseCreated);
            }
        }
    }
}