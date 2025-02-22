namespace FoodStuffs.Model.Events.MealPlans.Models;

public record GetMealPlanResponseExcludedShoppingItem(
    int Id,
    string Name,
    decimal Quantity);
