namespace FoodStuffs.Model.Search.GroceryItems.Models;

public record SearchGroceryItemsResultItem(
    int Id,
    string Name,
    bool IsOutOfStock,
    bool IsUnused,
    int InventoryQuantity,
    int RecipeCount,
    DateTimeOffset CreatedOn,
    DateTimeOffset ModifiedOn,
    List<SearchGroceryItemsResultItemStorageLocation> StorageLocations,
    List<SearchGroceryItemsResultItemGroceryStore> GroceryStores,
    SearchGroceryItemsResultItemGroceryAisle? GroceryAisle);
