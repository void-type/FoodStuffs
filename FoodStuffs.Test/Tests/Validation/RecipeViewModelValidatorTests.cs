using FoodStuffs.Model.Validation;
using FoodStuffs.Model.ViewModels;
using System.Linq;
using Xunit;

namespace FoodStuffs.Test.Tests.Validation
{
    public class RecipeViewModelValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" \n")]
        public void InvalidWhenDirectionsEmpty(string directions)
        {
            var recipe = new RecipeViewModel
            {
                Directions = directions
            };

            var validator = new RecipeViewModelValidator();

            var errors = validator.Validate(recipe);

            Assert.NotEmpty(errors.Where(x => x.FieldName == "Directions"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" \n")]
        public void InvalidWhenIngredientsEmpty(string ingredients)
        {
            var recipe = new RecipeViewModel
            {
                Ingredients = ingredients
            };

            var validator = new RecipeViewModelValidator();

            var errors = validator.Validate(recipe);

            Assert.NotEmpty(errors.Where(x => x.FieldName == "Ingredients"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" \n")]
        public void InvalidWhenRecipeNameEmpty(string recipeName)
        {
            var recipe = new RecipeViewModel
            {
                Name = recipeName
            };

            var validator = new RecipeViewModelValidator();

            var errors = validator.Validate(recipe);

            Assert.NotEmpty(errors.Where(x => x.FieldName == "Name"));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        private void InvalidWhenCookTimeNegative(int? time)
        {
            var recipe = new RecipeViewModel
            {
                CookTimeMinutes = time
            };

            var validator = new RecipeViewModelValidator();

            var errors = validator.Validate(recipe);

            Assert.NotEmpty(errors.Where(x => x.FieldName == "CookTimeMinutes"));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        private void InvalidWhenPrepTimeNegative(int? time)
        {
            var recipe = new RecipeViewModel
            {
                PrepTimeMinutes = time
            };

            var validator = new RecipeViewModelValidator();

            var errors = validator.Validate(recipe);

            Assert.NotEmpty(errors.Where(x => x.FieldName == "PrepTimeMinutes"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(1000)]
        private void ValidWhenCookTimeNullOrPositive(int? time)
        {
            var recipe = new RecipeViewModel
            {
                CookTimeMinutes = time
            };

            var validator = new RecipeViewModelValidator();

            var errors = validator.Validate(recipe);

            Assert.Empty(errors.Where(x => x.FieldName == "CookTimeMinutes"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        private void ValidWhenDirectionsNotEmpty(string directions)
        {
            var recipe = new RecipeViewModel
            {
                Directions = directions
            };

            var validator = new RecipeViewModelValidator();

            var errors = validator.Validate(recipe);

            Assert.Empty(errors.Where(x => x.FieldName == "Directions"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        private void ValidWhenIngredientsNotEmpty(string ingredients)
        {
            var recipe = new RecipeViewModel
            {
                Ingredients = ingredients
            };

            var validator = new RecipeViewModelValidator();

            var errors = validator.Validate(recipe);

            Assert.Empty(errors.Where(x => x.FieldName == "Ingredients"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(1000)]
        private void ValidWhenPrepTimeNullOrPositive(int? time)
        {
            var recipe = new RecipeViewModel
            {
                PrepTimeMinutes = time
            };

            var validator = new RecipeViewModelValidator();

            var errors = validator.Validate(recipe);

            Assert.Empty(errors.Where(x => x.FieldName == "PrepTimeMinutes"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
        private void ValidWhenRecipeNameNotEmpty(string recipeName)
        {
            var recipe = new RecipeViewModel
            {
                Name = recipeName
            };

            var validator = new RecipeViewModelValidator();

            var errors = validator.Validate(recipe);

            Assert.Empty(errors.Where(x => x.FieldName == "Name"));
        }
    }
}