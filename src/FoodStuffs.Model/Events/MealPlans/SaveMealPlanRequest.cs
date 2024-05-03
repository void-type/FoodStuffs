namespace FoodStuffs.Model.Events.MealPlans;

public record SaveMealPlanRequest(
    int Id,
    string Name,
    IEnumerable<int> RecipeIds,
    IEnumerable<SaveMealPlanRequestPantryIngredient> PantryIngredients);
