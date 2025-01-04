namespace FoodStuffs.Model.Events.Recipes.Models;

public record SaveRecipeRequestShoppingItem(
    int Id,
    int Quantity,
    int Order);
