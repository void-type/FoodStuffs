namespace FoodStuffs.Model.Events.MealPlans.Models;

public record SaveMealPlanRequestRecipe(
    int Id,
    int Order,
    bool IsComplete);
