namespace FoodStuffs.Model.Events.Recipes.Models;

public record GetRecipeResponseShoppingItem(
    int Id,
    string Name,
    decimal Quantity,
    int Order);
