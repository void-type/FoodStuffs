using FoodStuffs.Model.Events.GroceryItems;
using FoodStuffs.Model.Events.GroceryItems.Models;
using Xunit;

namespace FoodStuffs.Test;

public class SaveGroceryItemInventoryRequestValidatorTests
{
    [Fact]
    public void GroceryItemInventory_invalid_when_inventory_quantity_null()
    {
        var request = new SaveGroceryItemInventoryRequest(1, null);
        var validator = new SaveGroceryItemInventoryRequestValidator();
        var result = validator.Validate(request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Failures, x => x.UiHandle == "inventoryQuantity");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void GroceryItemInventory_invalid_when_inventory_quantity_negative(int quantity)
    {
        var request = new SaveGroceryItemInventoryRequest(1, quantity);
        var validator = new SaveGroceryItemInventoryRequestValidator();
        var result = validator.Validate(request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Failures, x => x.UiHandle == "inventoryQuantity");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    public void GroceryItemInventory_valid_when_inventory_quantity_non_negative(int quantity)
    {
        var request = new SaveGroceryItemInventoryRequest(1, quantity);
        var validator = new SaveGroceryItemInventoryRequestValidator();
        var result = validator.Validate(request);

        Assert.True(result.IsSuccess);
    }
}
