namespace FoodStuffs.Model.Events.Recipes.Models;

public record SaveRecipeRequest(
    int Id,
    string Name,
    string Directions,
    string Sides,
    int? CookTimeMinutes,
    int? PrepTimeMinutes,
    bool IsForMealPlanning,
    int MealPlanningSidesCount,
    List<SaveRecipeRequestGroceryItem> GroceryItems,
    List<string> Categories);
