using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Validation;
using System.Linq;
using Xunit;

namespace FoodStuffs.Test.Tests.Validation
{
    public class RecipeValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void InvalidWhenDirectionsEmpty(string directions)
        {
            var recipe = new Recipe
            {
                Directions = directions
            };

            var validator = new RecipeValidator();

            var errors = validator.Validate(recipe);

            Assert.NotEmpty(errors.Where(x => x.FieldName == "Directions"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void InvalidWhenIngredientsEmpty(string ingredients)
        {
            var recipe = new Recipe
            {
                Ingredients = ingredients
            };

            var validator = new RecipeValidator();

            var errors = validator.Validate(recipe);

            Assert.NotEmpty(errors.Where(x => x.FieldName == "Ingredients"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void InvalidWhenRecipeNameEmpty(string recipeName)
        {
            var recipe = new Recipe
            {
                Name = recipeName
            };

            var validator = new RecipeValidator();

            var errors = validator.Validate(recipe);

            Assert.NotEmpty(errors.Where(x => x.FieldName == "Name"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Lon_Nam! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        private void ValidWhenDirectionsNotEmpty(string directions)
        {
            var recipe = new Recipe
            {
                Directions = directions
            };

            var validator = new RecipeValidator();

            var errors = validator.Validate(recipe);

            Assert.Empty(errors.Where(x => x.FieldName == "Directions"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Lon_Nam! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        private void ValidWhenIngredientsNotEmpty(string ingredients)
        {
            var recipe = new Recipe
            {
                Ingredients = ingredients
            };

            var validator = new RecipeValidator();

            var errors = validator.Validate(recipe);

            Assert.Empty(errors.Where(x => x.FieldName == "Ingredients"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Lon_Nam! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        private void ValidWhenRecipeNameNotEmpty(string recipeName)
        {
            var recipe = new Recipe
            {
                Name = recipeName
            };

            var validator = new RecipeValidator();

            var errors = validator.Validate(recipe);

            Assert.Empty(errors.Where(x => x.FieldName == "Name"));
        }
    }
}