using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Validation;
using System.Linq;
using Xunit;

namespace FoodStuffs.Test.Tests.Validation
{
    public class CategoryValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void InvalidWhenCategoryNameEmpty(string categoryName)
        {
            var category = new Category
            {
                Name = categoryName
            };

            var validator = new CategoryValidator();

            var errors = validator.Validate(category);

            Assert.NotEmpty(errors.Where(x => x.FieldName == "Name"));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("Really Long_Name! with special characters !@#$%^&*():.,")]
        private void ValidWhenCategoryNameNotEmpty(string categoryName)
        {
            var category = new Category
            {
                Name = categoryName
            };

            var validator = new CategoryValidator();

            var errors = validator.Validate(category);

            Assert.Empty(errors.Where(x => x.FieldName == "Name"));
        }
    }
}