namespace FoodStuffs.Model.Events.GroceryItems.Models;

public record GetGroceryItemResponseRecipe(
    int Id,
    string Name,
    string Slug,
    string? Image);
