using Core.Model.Actions.Chain;
using FoodStuffs.Data.Models;
using FoodStuffs.Data.Services;
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
        public void UpdateRecipeAndRelations()
        {
            var responder = MockFactory.GetResponder;
            var then = MockFactory.EarlyDateTimeService;
            var now = MockFactory.LateDateTimeService;

            using (var data = new FoodStuffsListData())
            {
                data.Users.Add(MockFactory.User1);
                data.Users.Add(MockFactory.User2);

                data.Categories.Add(MockFactory.Category1);
                data.Categories.Add(MockFactory.Category2);
                data.Categories.Add(MockFactory.Category3);

                var recipeToUpdate = MockFactory.Recipe1;

                data.Recipes.Add(recipeToUpdate);

                data.Recipes.Add(MockFactory.Recipe2);

                data.CategoryRecipes.AddRange(new List<ICategoryRecipe> {
                    new CategoryRecipe
                    {
                        RecipeId = 1,
                        CategoryId = 1
                    },

                    new CategoryRecipe
                    {
                        RecipeId = 1,
                        CategoryId = 2
                    },

                    new CategoryRecipe
                    {
                        RecipeId = 1,
                        CategoryId = 3
                    },

                    new CategoryRecipe
                    {
                        RecipeId = 2,
                        CategoryId = 2
                    },

                    new CategoryRecipe
                    {
                        RecipeId = 2,
                        CategoryId = 3
                    }
                });

                // Force relations in list database
                if (data.GetType() == typeof(FoodStuffsListData))
                {
                    foreach (var category in data.Categories.Stored)
                    {
                        category.CategoryRecipe = data.CategoryRecipes.Stored.Where(cr => cr.CategoryId == category.Id)
                            .ToList();
                    }

                    foreach (var categoryRecipe in data.CategoryRecipes.Stored)
                    {
                        categoryRecipe.Category =
                            data.Categories.Stored.FirstOrDefault(c => c.Id == categoryRecipe.CategoryId);

                        categoryRecipe.Recipe =
                            data.Recipes.Stored.FirstOrDefault(r => r.Id == categoryRecipe.RecipeId);
                    }

                    foreach (var recipe in data.Recipes.Stored)
                    {
                        recipe.CategoryRecipe =
                            data.CategoryRecipes.Stored.Where(cr => cr.RecipeId == recipe.Id).ToList();
                    }
                }

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
                    ModifiedOn = new DateTime(2222, 12, 12),
                    CreatedOn = new DateTime(1903, 12, 12),
                    ModifiedByUserId = 52,
                    CreatedByUserId = 53
                };

                new ActionChain(responder)
                    .Execute(new UpdateRecipe(data, now, newRecipeViewModel, 2));

                var changedRecipe = data.Recipes.Stored.GetById(recipeToUpdate.Id);

                Assert.True(responder.ResponseCreated);

                Assert.Equal("ChangedRecipeName 1", changedRecipe.Name);
                Assert.Equal(4, changedRecipe.PrepTimeMinutes);
                Assert.Equal(then.Moment, changedRecipe.CreatedOn);
                Assert.Equal(now.Moment, changedRecipe.ModifiedOn);
                Assert.Equal(1, changedRecipe.CreatedByUserId);
                Assert.Equal(2, changedRecipe.ModifiedByUserId);

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
            var responder = MockFactory.GetResponder;
            var now = MockFactory.LateDateTimeService;

            var recipeViewModel = new RecipeViewModel
            {
                Id = 2,
                Categories = new List<string>
                {
                    "Category1"
                }
            };

            using (var data = new FoodStuffsEfMemoryData())
            {
                new ActionChain(responder)
                    .Execute(new UpdateRecipe(data, now, recipeViewModel, 1));
            }

            Assert.True(responder.ResponseCreated);
            Assert.Single(responder.ValidationErrors);
        }
    }
}