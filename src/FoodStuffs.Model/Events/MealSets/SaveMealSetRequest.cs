namespace FoodStuffs.Model.Events.MealSets;

public record SaveMealSetRequest(
    int Id,
    string Name,
    IEnumerable<int> RecipeIds,
    IEnumerable<SaveMealSetRequestPantryIngredient> PantryIngredients);
