using FoodStuffs.Model.Events.Recipes;
using System.Linq;
using Xunit;

namespace FoodStuffs.Test.Model.Events
{
    public class SaveRecipeRequestValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" \n")]
        public void Recipe_invalid_when_directions_empty(string directions)
        {
            var recipe = new SaveRecipe.Request(0, "null", "null", directions, 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures.Where(x => x.UiHandle == "directions"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        public void Recipe_valid_when_directions_not_empty(string directions)
        {
            var recipe = new SaveRecipe.Request(0, "null", "null", directions, 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" \n")]
        public void Recipe_invalid_when_ingredients_empty(string ingredients)
        {
            var recipe = new SaveRecipe.Request(0, "null", ingredients, "null", 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures.Where(x => x.UiHandle == "ingredients"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        public void Recipe_valid_when_ingredients_not_empty(string ingredients)
        {
            var recipe = new SaveRecipe.Request(0, "null", ingredients, "null", 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" \n")]
        public void Recipe_invalid_when_name_empty(string recipeName)
        {
            var recipe = new SaveRecipe.Request(0, recipeName, "null", "null", 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures.Where(x => x.UiHandle == "name"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        public void Recipe_valid_when_name_not_empty(string recipeName)
        {
            var recipe = new SaveRecipe.Request(0, recipeName, "null", "null", 10, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        public void Recipe_invalid_when_cook_time_negative(int? time)
        {
            var recipe = new SaveRecipe.Request(0, "null", "null", "null", time, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures.Where(x => x.UiHandle == "cookTimeMinutes"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(1000)]
        public void Recipe_valid_when_cook_time_null_or_positive(int? time)
        {
            var recipe = new SaveRecipe.Request(0, "null", "null", "null", time, 10, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        public void Recipe_invalid_when_prep_time_negative(int? time)
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
        public void Recipe_valid_when_prep_time_null_or_positive(int? time)
        {
            var recipe = new SaveRecipe.Request(0, "null", "null", "null", 10, time, new string[] { });
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        public void Recipe_is_valid_with_name_ingredients_directions_at_minimum(string testString)
        {
            var recipe = new SaveRecipe.Request(0, testString, testString, testString, null, null, null);
            var validator = new SaveRecipe.RequestValidator();
            var result = validator.Validate(recipe);

            Assert.True(result.IsSuccess);
        }
    }
}
