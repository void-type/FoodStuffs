namespace FoodStuffs.Model.Events.MealPlans;

public record SaveMealPlanRequest(
    int Id,
    string Name,
    List<SaveMealPlanRequestRecipe> Recipes,
    List<SaveMealPlanRequestPantryIngredient> PantryIngredients);
