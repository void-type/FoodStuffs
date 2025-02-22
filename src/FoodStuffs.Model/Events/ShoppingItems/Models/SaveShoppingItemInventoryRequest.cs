namespace FoodStuffs.Model.Events.ShoppingItems.Models;

public record SaveShoppingItemInventoryRequest(
    int Id,
    int InventoryQuantity);
