using FoodStuffs.Model.Events.GroceryItems;
using FoodStuffs.Model.Events.GroceryItems.Models;
using Xunit;

namespace FoodStuffs.Test;

public class SaveGroceryItemRequestValidatorTests
{
    [Fact]
    public void GroceryItem_invalid_when_inventory_quantity_null()
    {
        var request = new SaveGroceryItemRequest(0, "Item", null, null, [], []);
        var validator = new SaveGroceryItemRequestValidator();
        var result = validator.Validate(request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Failures, x => x.UiHandle == "inventoryQuantity");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void GroceryItem_invalid_when_inventory_quantity_negative(int quantity)
    {
        var request = new SaveGroceryItemRequest(0, "Item", quantity, null, [], []);
        var validator = new SaveGroceryItemRequestValidator();
        var result = validator.Validate(request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Failures, x => x.UiHandle == "inventoryQuantity");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    public void GroceryItem_valid_when_inventory_quantity_non_negative(int quantity)
    {
        var request = new SaveGroceryItemRequest(0, "Item", quantity, null, [], []);
        var validator = new SaveGroceryItemRequestValidator();
        var result = validator.Validate(request);

        Assert.True(result.IsSuccess);
    }
}
