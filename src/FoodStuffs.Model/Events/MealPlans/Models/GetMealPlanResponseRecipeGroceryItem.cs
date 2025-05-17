namespace FoodStuffs.Model.Events.MealPlans.Models;

public record GetMealPlanResponseRecipeGroceryItem(
    int Id,
    string Name,
    int InventoryQuantity,
    decimal Quantity,
    int Order,
    int? GroceryDepartmentId);
