namespace FoodStuffs.Model.Search.GroceryItems.Models;

public record SuggestGroceryItemsResultItem(
    int Id,
    string Name,
    string Slug,
    string? Image);
