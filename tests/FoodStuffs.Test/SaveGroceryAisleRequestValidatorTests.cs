using FoodStuffs.Model.Events.GroceryAisles;
using FoodStuffs.Model.Events.GroceryAisles.Models;
using Xunit;

namespace FoodStuffs.Test;

public class SaveGroceryAisleRequestValidatorTests
{
    [Fact]
    public void GroceryAisle_invalid_when_order_null()
    {
        var request = new SaveGroceryAisleRequest(0, "Aisle", null);
        var validator = new SaveGroceryAisleRequestValidator();
        var result = validator.Validate(request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Failures, x => x.UiHandle == "order");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    public void GroceryAisle_valid_when_order_provided(int order)
    {
        var request = new SaveGroceryAisleRequest(0, "Aisle", order);
        var validator = new SaveGroceryAisleRequestValidator();
        var result = validator.Validate(request);

        Assert.True(result.IsSuccess);
    }
}
