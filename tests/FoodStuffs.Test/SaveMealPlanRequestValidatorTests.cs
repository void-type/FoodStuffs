using FoodStuffs.Model.Events.MealPlans;
using FoodStuffs.Model.Events.MealPlans.Models;
using Xunit;

namespace FoodStuffs.Test;

public class SaveMealPlanRequestValidatorTests
{
    [Fact]
    public void MealPlan_invalid_when_excluded_grocery_item_quantity_null()
    {
        var excludedItems = new List<SaveMealPlanRequestExcludedGroceryItem>
        {
            new(1, null),
        };
        var request = new SaveMealPlanRequest(0, "Plan", excludedItems, null);
        var validator = new SaveMealPlanRequestValidator();
        var result = validator.Validate(request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Failures, x => x.UiHandle == "excludedGroceryItemsQuantity");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void MealPlan_invalid_when_excluded_grocery_item_quantity_not_positive(int quantity)
    {
        var excludedItems = new List<SaveMealPlanRequestExcludedGroceryItem>
        {
            new(1, quantity),
        };
        var request = new SaveMealPlanRequest(0, "Plan", excludedItems, null);
        var validator = new SaveMealPlanRequestValidator();
        var result = validator.Validate(request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Failures, x => x.UiHandle == "excludedGroceryItemsQuantity");
    }

    [Fact]
    public void MealPlan_valid_when_excluded_grocery_item_quantity_positive()
    {
        var excludedItems = new List<SaveMealPlanRequestExcludedGroceryItem>
        {
            new(1, 1),
        };
        var request = new SaveMealPlanRequest(0, "Plan", excludedItems, null);
        var validator = new SaveMealPlanRequestValidator();
        var result = validator.Validate(request);

        Assert.True(result.IsSuccess);
    }
}
