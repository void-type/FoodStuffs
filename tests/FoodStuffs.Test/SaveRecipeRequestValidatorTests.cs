using FoodStuffs.Model.Events.Recipes;
using Xunit;

namespace FoodStuffs.Test;

public class SaveRecipeRequestValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \n")]
    public void Recipe_valid_when_directions_empty(string? directions)
    {
        var recipe = new SaveRecipeRequest(0, "null", directions!, string.Empty, 10, 10, false, [], [], []);
        var validator = new SaveRecipeRequestValidator();
        var result = validator.Validate(recipe);

        Assert.False(result.IsFailed);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
    public void Recipe_valid_when_directions_not_empty(string directions)
    {
        var recipe = new SaveRecipeRequest(0, "null", directions, string.Empty, 10, 10, false, [], [], []);
        var validator = new SaveRecipeRequestValidator();
        var result = validator.Validate(recipe);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \n")]
    public void Recipe_invalid_when_ingredient_name_empty(string? ingredientName)
    {
        var recipe = new SaveRecipeRequest(0, "null", "null", string.Empty, 10, 10, false, [new SaveRecipeRequestIngredient(ingredientName!, 2, 1, false)], [], []);
        var validator = new SaveRecipeRequestValidator();
        var result = validator.Validate(recipe);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Failures, x => x.UiHandle == "ingredients");
    }

    [Theory]
    [InlineData("1")]
    [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
    public void Recipe_valid_when_ingredients_not_empty(string ingredients)
    {
        var recipe = new SaveRecipeRequest(0, "null", "null", string.Empty, 10, 10, false, [new SaveRecipeRequestIngredient(ingredients, 1, 2, false)], [], []);
        var validator = new SaveRecipeRequestValidator();
        var result = validator.Validate(recipe);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \n")]
    public void Recipe_invalid_when_name_empty(string? recipeName)
    {
        var recipe = new SaveRecipeRequest(0, recipeName!, "null", string.Empty, 10, 10, false, [], [], []);
        var validator = new SaveRecipeRequestValidator();
        var result = validator.Validate(recipe);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Failures, x => x.UiHandle == "name");
    }

    [Theory]
    [InlineData("1")]
    [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
    public void Recipe_valid_when_name_not_empty(string recipeName)
    {
        var recipe = new SaveRecipeRequest(0, recipeName, "null", string.Empty, 10, 10, false, [], [], []);
        var validator = new SaveRecipeRequestValidator();
        var result = validator.Validate(recipe);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-1000)]
    public void Recipe_invalid_when_cook_time_negative(int? time)
    {
        var recipe = new SaveRecipeRequest(0, "null", "null", string.Empty, time, 10, false, [], [], []);
        var validator = new SaveRecipeRequestValidator();
        var result = validator.Validate(recipe);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Failures, x => x.UiHandle == "cookTimeMinutes");
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1000)]
    public void Recipe_valid_when_cook_time_null_or_positive(int? time)
    {
        var recipe = new SaveRecipeRequest(0, "null", "null", string.Empty, time, 10, false, [], [], []);
        var validator = new SaveRecipeRequestValidator();
        var result = validator.Validate(recipe);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-1000)]
    public void Recipe_invalid_when_prep_time_negative(int? time)
    {
        var recipe = new SaveRecipeRequest(0, "null", "null", string.Empty, 10, time, false, [], [], []);
        var validator = new SaveRecipeRequestValidator();
        var result = validator.Validate(recipe);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Failures, x => x.UiHandle == "prepTimeMinutes");
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1000)]
    public void Recipe_valid_when_prep_time_null_or_positive(int? time)
    {
        var recipe = new SaveRecipeRequest(0, "null", "null", string.Empty, 10, time, false, [], [], []);
        var validator = new SaveRecipeRequestValidator();
        var result = validator.Validate(recipe);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("Really Long_Name! @ llsla;lad;lsf asdflk;asdfjkl;jkl;asd")]
    public void Recipe_is_valid_with_name_ingredients_directions_at_minimum(string testString)
    {
        var recipe = new SaveRecipeRequest(0, testString, testString, string.Empty, null, null, false, [new SaveRecipeRequestIngredient(testString, 12, 1, false)], [], []);
        var validator = new SaveRecipeRequestValidator();
        var result = validator.Validate(recipe);

        Assert.True(result.IsSuccess);
    }
}
