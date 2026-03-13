namespace FoodStuffs.Model.Events.GroceryStores.Models;

public record ListGroceryStoresResponse(
    int Id,
    string Name,
    int GroceryItemCount);
