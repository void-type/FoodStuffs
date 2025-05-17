namespace FoodStuffs.Model.Events.GroceryItems.Models;

public record SaveGroceryItemRequest(
    int Id,
    string Name,
    int InventoryQuantity,
    int? GroceryAisleId,
    List<string> PantryLocations);
