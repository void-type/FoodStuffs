namespace FoodStuffs.Model.Events.GroceryItems.Models;

public record ListGroceryItemsResponse(
    int Id,
    string Name,
    int InventoryQuantity,
    List<string> StorageLocations,
    int? GroceryAisleId,
    int RecipeCount);
