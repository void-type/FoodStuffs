namespace FoodStuffs.Model.Events.GroceryItems.Models;

public record SaveGroceryItemInventoryRequest(
    int Id,
    int InventoryQuantity);
