namespace FoodStuffs.Model.Events.MealPlans;

public record GetMealPlanResponseRecipeIngredient(
    string Name,
    decimal Quantity,
    int Order,
    bool IsCategory);
