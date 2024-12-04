namespace FoodStuffs.Model.Events.ShoppingItems;

public record GetShoppingItemResponseRecipe(
    int Id,
    string Name,
    string? Image);
