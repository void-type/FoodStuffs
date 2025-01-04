namespace FoodStuffs.Model.Events.MealPlans.Models;

public record GetMealPlanResponseRecipe(
    int Id,
    string Name,
    int Order,
    string? Image,
    List<string> Categories,
    List<GetMealPlanResponseRecipeShoppingItem> ShoppingItems);
