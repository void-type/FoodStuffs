namespace FoodStuffs.Model.Events.MealPlans.Models;

public record SaveMealPlanRequest(
    int Id,
    string Name,
    List<SaveMealPlanRequestPantryShoppingItem> PantryShoppingItems,
    List<SaveMealPlanRequestRecipe> Recipes);
