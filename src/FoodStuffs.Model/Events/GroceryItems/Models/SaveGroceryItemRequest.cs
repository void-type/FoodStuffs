namespace FoodStuffs.Model.Events.GroceryItems.Models;

public record SaveGroceryItemRequest(
    int Id,
    string Name,
    int InventoryQuantity,
    int? GroceryDepartmentId,
    List<string> PantryLocations);
