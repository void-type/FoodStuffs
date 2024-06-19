namespace FoodStuffs.Model.Events.Recipes;

public record GetRecipeResponseShoppingItem(
    int Id,
    string Name,
    decimal Quantity,
    int Order);
