namespace FoodStuffs.Model.Events.MealPlans.Models;

public record GetMealPlanResponseRecipeShoppingItem(
    int Id,
    string Name,
    decimal Quantity);
