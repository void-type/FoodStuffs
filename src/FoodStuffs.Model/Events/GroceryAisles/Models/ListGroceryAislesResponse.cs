namespace FoodStuffs.Model.Events.GroceryAisles.Models;

public record ListGroceryAislesResponse(
    int Id,
    string Name,
    int Order,
    int GroceryItemCount);
