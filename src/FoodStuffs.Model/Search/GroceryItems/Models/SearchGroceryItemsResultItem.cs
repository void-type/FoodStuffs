namespace FoodStuffs.Model.Search.GroceryItems.Models;

public record SearchGroceryItemsResultItem(
    int Id,
    string Name,
    bool IsOutOfStock,
    bool IsUnused,
    int InventoryQuantity,
    int RecipeCount,
    DateTimeOffset CreatedOn,
    List<SearchGroceryItemsResultItemStorageLocation> StorageLocations,
    SearchGroceryItemsResultItemGroceryAisle? GroceryAisle);
