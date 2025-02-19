namespace FoodStuffs.Model.Events.ShoppingItems.Models;

public record SaveShoppingItemRequest(
    int Id,
    string Name,
    int InventoryQuantity,
    int? GroceryDepartmentId);
