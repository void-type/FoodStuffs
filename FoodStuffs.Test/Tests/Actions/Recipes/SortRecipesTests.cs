using Core.Model.Actions.Chain;
using FoodStuffs.Model.Actions.Recipes;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using FoodStuffs.Test.Mocks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FoodStuffs.Test.Tests.Actions.Recipes
{
    public class SortRecipesTests
    {
        [Theory]
        [InlineData("ascending", "Recipe1")]
        [InlineData("descending", "Recipe3")]
        [InlineData("chronological", "Recipe2")]
        [InlineData("not found", "Recipe2")]
        [InlineData(null, "Recipe2")]
        public void SortAscending(string sortType, string expectedName)
        {
            var responder = MockFactory.Responder;

            var recipe2 = MockFactory.Recipe2;
            recipe2.Id = 1;

            var list = new List<Recipe>
            {
                recipe2,
                MockFactory.Recipe3,
                MockFactory.Recipe1
            }
                .ToViewModels()
                .ToList();

            new ActionChain(responder)
                .Execute(new SortRecipes(sortType, list));

            Assert.Equal(expectedName, list.First().Name);
        }
    }
}