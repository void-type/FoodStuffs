using Core.Model.Actions.Chain;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.ViewModels;
using FoodStuffs.Test.Mocks;
using System.Collections.Generic;
using Xunit;

namespace FoodStuffs.Test.Actions.Recipes
{
    public class SearchRecipesTests
    {
        [Fact]
        public void SearchAllRecipesEmpty()
        {
            var responder = MockFactory.Responder;

            using (var data = MockFactory.FoodStuffsData())
            {
                data.Users.Add(MockFactory.User1);

                var context = new List<IRecipeViewModel>();

                new ActionChain(responder)
                    .Execute(new SearchRecipes(data, null, null, context));

                Assert.False(responder.ResponseCreated);
                Assert.Empty(context);
            }
        }

        [Theory]
        [InlineData(null, 3)]
        [InlineData("cat", 3)]
        [InlineData("1", 1)]
        [InlineData("cat 1 3", 1)]
        [InlineData("1 2", 0)]
        public void SearchRecipesByCategory(string categorySearch, int expectedFound)
        {
            var responder = MockFactory.Responder;

            using (var data = MockFactory.FoodStuffsData())
            {
                var recipe1 = MockFactory.Recipe1;
                var recipe2 = MockFactory.Recipe2;
                var recipe3 = MockFactory.Recipe3;

                var category1 = MockFactory.Category1;
                var category2 = MockFactory.Category2;
                var category3 = MockFactory.Category3;

                data.Users.Add(MockFactory.User1);

                data.Recipes.Add(recipe1);
                data.Recipes.Add(recipe2);
                data.Recipes.Add(recipe3);

                data.Categories.Add(category1);
                data.Categories.Add(category2);
                data.Categories.Add(category3);

                var categoryRecipe1 = new CategoryRecipe
                {
                    CategoryId = 11,
                    RecipeId = 11,
                };

                var categoryRecipe2 = new CategoryRecipe
                {
                    CategoryId = 12,
                    RecipeId = 12,
                };

                var categoryRecipe3 = new CategoryRecipe
                {
                    CategoryId = 13,
                    RecipeId = 13,
                };

                var categoryRecipe4 = new CategoryRecipe
                {
                    CategoryId = 13,
                    RecipeId = 11,
                };

                data.CategoryRecipes.Add(categoryRecipe1);
                data.CategoryRecipes.Add(categoryRecipe2);
                data.CategoryRecipes.Add(categoryRecipe3);
                data.CategoryRecipes.Add(categoryRecipe4);

                data.SaveChanges();

                var context = new List<IRecipeViewModel>();

                new ActionChain(responder)
                    .Execute(new SearchRecipes(data, null, categorySearch, context));

                Assert.False(responder.ResponseCreated);
                Assert.Equal(expectedFound, context.Count);
            }
        }

        [Theory]
        [InlineData("recipe", 3)]
        [InlineData("cipe", 3)]
        [InlineData("", 3)]
        [InlineData(null, 3)]
        [InlineData("1", 1)]
        public void SearchRecipesByName(string nameSearch, int expectedFound)
        {
            var responder = MockFactory.Responder;

            using (var data = MockFactory.FoodStuffsData())
            {
                data.Users.Add(MockFactory.User1);
                data.Recipes.Add(MockFactory.Recipe1);
                data.Recipes.Add(MockFactory.Recipe2);
                data.Recipes.Add(MockFactory.Recipe3);

                data.SaveChanges();

                var context = new List<IRecipeViewModel>();

                new ActionChain(responder)
                    .Execute(new SearchRecipes(data, nameSearch, null, context));

                Assert.False(responder.ResponseCreated);
                Assert.Equal(expectedFound, context.Count);
            }
        }

        [Fact]
        public void SearchRecipesNotEmpty()
        {
            var responder = MockFactory.Responder;

            using (var data = MockFactory.FoodStuffsData())
            {
                data.Users.Add(MockFactory.User1);
                data.Recipes.Add(MockFactory.Recipe1);
                data.Recipes.Add(MockFactory.Recipe2);

                data.SaveChanges();

                var context = new List<IRecipeViewModel>();

                new ActionChain(responder)
                    .Execute(new SearchRecipes(data, null, null, context));

                Assert.False(responder.ResponseCreated);
                Assert.Equal(2, context.Count);
            }
        }
    }
}