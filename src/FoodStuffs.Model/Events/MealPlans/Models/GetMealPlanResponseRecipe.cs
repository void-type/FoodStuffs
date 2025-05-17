namespace FoodStuffs.Model.Events.MealPlans.Models;

public record GetMealPlanResponseRecipe(
    int Id,
    string Name,
    int Order,
    bool IsComplete,
    string? Image,
    List<string> Categories,
    List<GetMealPlanResponseRecipeGroceryItem> GroceryItems);
