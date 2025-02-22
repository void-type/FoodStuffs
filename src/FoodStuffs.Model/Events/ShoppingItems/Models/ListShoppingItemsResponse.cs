namespace FoodStuffs.Model.Events.ShoppingItems.Models;

public record ListShoppingItemsResponse(
    int Id,
    string Name,
    int InventoryQuantity,
    List<string> PantryLocations);
