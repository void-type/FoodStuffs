namespace FoodStuffs.Model.Events.ShoppingItems.Models;

public record GetShoppingItemResponseRecipe(
    int Id,
    string Name,
    string Slug,
    string? Image);
