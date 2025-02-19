namespace FoodStuffs.Model.Events.MealPlans.Models;

public record GetMealPlanResponseRecipeShoppingItem(
    int Id,
    string Name,
    int InventoryQuantity,
    decimal Quantity,
    int? GroceryDepartmentId);
