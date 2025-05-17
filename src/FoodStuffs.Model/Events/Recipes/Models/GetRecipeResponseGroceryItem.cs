namespace FoodStuffs.Model.Events.Recipes.Models;

public record GetRecipeResponseGroceryItem(
    int Id,
    string Name,
    int InventoryQuantity,
    decimal Quantity,
    int Order);
