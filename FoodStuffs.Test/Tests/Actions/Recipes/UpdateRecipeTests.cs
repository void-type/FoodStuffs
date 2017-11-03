using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Data.Test;
using FoodStuffs.Model.Actions.Core.Chain;
using FoodStuffs.Model.Actions.Recipes;
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
            var then = MockFactory.EarlyDateTimeService;
            var now = MockFactory.LateDateTimeService;

            using (var data = new FoodStuffsListData())
            {
                data.Users.Add(new User
                {
                    Id = 1
                });

                data.Users.Add(new User
                {
                    Id = 2
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

                data.Categories.Add(new Category
                {
                    Id = 3,
                    Name = "Category3"
                });

                var recipeToUpdate = new Recipe
                {
                    Id = 1,
                    Name = "Recipe1",
                    CookTimeMinutes = 2,
                    PrepTimeMinutes = 2,
                    CreatedOn = then.Moment,
                    ModifiedOn = then.Moment,
                    CreatedByUserId = 1
                };

                data.Recipes.Add(recipeToUpdate);

                data.Recipes.Add(new Recipe
                {
                    Id = 2,
                    Name = "Recipe2",
                    CookTimeMinutes = 2,
                    PrepTimeMinutes = 2
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

                data.CategoryRecipes.Add(new CategoryRecipe
                {
                    RecipeId = 1,
                    CategoryId = 3
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

                // Force relations in list database
                foreach (var category in data.Categories.Stored)
                {
                    category.CategoryRecipe = data.CategoryRecipes.Stored.Where(cr => cr.CategoryId == category.Id).ToList();
                }

                foreach (var categoryRecipe in data.CategoryRecipes.Stored)
                {
                    categoryRecipe.Category = data.Categories.Stored.FirstOrDefault(c => c.Id == categoryRecipe.CategoryId);

                    categoryRecipe.Recipe = data.Recipes.Stored.FirstOrDefault(r => r.Id == categoryRecipe.RecipeId);
                }

                foreach (var recipe in data.Recipes.Stored)
                {
                    recipe.CategoryRecipe = data.CategoryRecipes.Stored.Where(cr => cr.RecipeId == recipe.Id).ToList();
                }

                data.SaveChanges();

                var responder = MockFactory.Responder;

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

                Assert.False(responder.ResponseCreated);

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
            var responder = MockFactory.Responder;
            var now = MockFactory.LateDateTimeService;

            var recipeViewModel = new RecipeViewModel
            {
                Id = 2,
                Categories = new List<string>
                {
                    "Category1"
                }
            };

            using (var data = new FoodStuffsMemoryData())
            {
                new ActionChain(responder)
                    .Execute(new UpdateRecipe(data, now, recipeViewModel, 1));
            }

            Assert.True(responder.ResponseCreated);
            Assert.Equal(1, responder.ValidationErrors.Count);
        }
    }
}