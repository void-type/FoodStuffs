namespace FoodStuffs.Model.Events.MealPlans.Models;

public record SaveMealPlanRequest(
    int Id,
    string Name,
    List<SaveMealPlanRequestExcludedShoppingItem> ExcludedShoppingItems,
    List<SaveMealPlanRequestRecipe> Recipes);
