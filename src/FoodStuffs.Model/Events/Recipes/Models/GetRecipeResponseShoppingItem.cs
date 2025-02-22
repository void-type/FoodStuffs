namespace FoodStuffs.Model.Events.Recipes.Models;

public record GetRecipeResponseShoppingItem(
    int Id,
    string Name,
    int InventoryQuantity,
    decimal Quantity,
    int Order);
