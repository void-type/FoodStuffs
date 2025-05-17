namespace FoodStuffs.Model.Events.GroceryItems.Models;

public record ListGroceryItemsResponse(
    int Id,
    string Name,
    int InventoryQuantity,
    List<string> PantryLocations,
    int? GroceryAisleId,
    int RecipeCount);
