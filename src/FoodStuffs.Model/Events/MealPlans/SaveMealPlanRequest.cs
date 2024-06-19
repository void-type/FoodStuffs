namespace FoodStuffs.Model.Events.MealPlans;

public record SaveMealPlanRequest(
    int Id,
    string Name,
    List<SaveMealPlanRequestPantryShoppingItem> PantryShoppingItems,
    List<SaveMealPlanRequestRecipe> Recipes);
