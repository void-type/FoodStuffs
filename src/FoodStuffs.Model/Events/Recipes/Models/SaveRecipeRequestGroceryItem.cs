namespace FoodStuffs.Model.Events.Recipes.Models;

public record SaveRecipeRequestGroceryItem(
    int Id,
    int Quantity,
    int Order);
