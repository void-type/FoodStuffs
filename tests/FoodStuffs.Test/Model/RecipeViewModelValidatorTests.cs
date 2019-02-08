using FoodStuffs.Model.Domain.Recipes;
using System.Linq;
using Xunit;

namespace FoodStuffs.Test.Model
{
    public class RecipeViewModelValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" \n")]
        public void InvalidWhenDirectionsEmpty(string directions)
        {
            var recipe = new SaveRecipe.Request(0, "null", "null", directions, 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures.Where(x => x.UiHandle == "directions"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" \n")]
        public void InvalidWhenIngredientsEmpty(string ingredients)
        {
            var recipe = new SaveRecipe.Request(0, "null", ingredients, "null", 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures.Where(x => x.UiHandle == "ingredients"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" \n")]
        public void InvalidWhenRecipeNameEmpty(string recipeName)
        {
            var recipe = new SaveRecipe.Request(0, recipeName, "null", "null", 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures.Where(x => x.UiHandle == "name"));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        private void InvalidWhenCookTimeNegative(int? time)
        {
            var recipe = new SaveRecipe.Request(0, "null", "null", "null", time, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures.Where(x => x.UiHandle == "cookTimeMinutes"));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        private void InvalidWhenPrepTimeNegative(int? time)
        {
            var recipe = new SaveRecipe.Request(0, "null", "null", "null", 10, time, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures.Where(x => x.UiHandle == "prepTimeMinutes"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(1000)]
        private void ValidWhenCookTimeNullOrPositive(int? time)
        {
            var recipe = new SaveRecipe.Request(0, "null", "null", "null", time, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Failures.Where(x => x.UiHandle == "cookTimeMinutes"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        private void ValidWhenDirectionsNotEmpty(string directions)
        {
            var recipe = new SaveRecipe.Request(0, "null", "null", directions, 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Failures.Where(x => x.UiHandle == "directions"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        private void ValidWhenIngredientsNotEmpty(string ingredients)
        {
            var recipe = new SaveRecipe.Request(0, "null", ingredients, "null", 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Failures.Where(x => x.UiHandle == "ingredients"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(1000)]
        private void ValidWhenPrepTimeNullOrPositive(int? time)
        {
            var recipe = new SaveRecipe.Request(0, "null", "null", "null", 10, time, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Failures.Where(x => x.UiHandle == "prepTimeMinutes"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        private void ValidWhenRecipeNameNotEmpty(string recipeName)
        {
            var recipe = new SaveRecipe.Request(0, recipeName, "null", "null", 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Failures.Where(x => x.UiHandle == "name"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        private void ValidWithMinimumInfo(string testString)
        {
            var recipe = new SaveRecipe.Request(0, testString, testString, testString, null, null, null);
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Failures);
        }
    }
}
