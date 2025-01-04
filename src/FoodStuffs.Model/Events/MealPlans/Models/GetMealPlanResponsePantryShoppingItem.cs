namespace FoodStuffs.Model.Events.MealPlans.Models;

public record GetMealPlanResponsePantryShoppingItem(
    int Id,
    string Name,
    decimal Quantity);
