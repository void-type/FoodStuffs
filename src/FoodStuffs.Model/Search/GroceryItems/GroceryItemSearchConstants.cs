using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Search.GroceryItems;

public static class GroceryItemSearchConstants
{
    public const string IndexName = "GroceryItem";

    public const string FieldId = nameof(GroceryItem.Id);
    public const string FieldName = nameof(GroceryItem.Name);
    public const string FieldNamePrefix = nameof(GroceryItem.Name) + "Prefix";
    public const string FieldCreatedOn = nameof(GroceryItem.CreatedOn);
    public const string FieldModifiedOn = nameof(GroceryItem.ModifiedOn);
    public const string FieldInventoryQuantity = nameof(GroceryItem.InventoryQuantity);
    public const string FieldRecipeCount = "RecipeCount";
    public const string FieldIsOutOfStock = "IsOutOfStock";
    public const string FieldIsUnused = "IsUnused";
    public const string FieldStorageLocationIds = nameof(GroceryItem.StorageLocations);
    public const string FieldStorageLocationsJson = nameof(GroceryItem.StorageLocations) + "Json";
    public const string FieldGroceryAisleId = nameof(GroceryItem.GroceryAisle);
    public const string FieldGroceryAisleJson = nameof(GroceryItem.GroceryAisle) + "Json";
    public const string FieldGroceryStoreIds = nameof(GroceryItem.GroceryStores);
    public const string FieldGroceryStoresJson = nameof(GroceryItem.GroceryStores) + "Json";
}
