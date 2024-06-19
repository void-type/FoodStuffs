namespace FoodStuffs.Model.Events.MealPlans;

public record GetMealPlanResponseRecipeShoppingItem(
    int Id,
    string Name,
    decimal Quantity);
