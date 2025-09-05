using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Search.GroceryItems;

public static class GroceryItemSearchConstants
{
    public const string INDEX_NAME = "GroceryItem";

    public const string FIELD_ID = nameof(GroceryItem.Id);
    public const string FIELD_NAME = nameof(GroceryItem.Name);
    public const string FIELD_NAME_PREFIX = nameof(GroceryItem.Name) + "Prefix";
    public const string FIELD_CREATED_ON = nameof(GroceryItem.CreatedOn);
    public const string FIELD_INVENTORY_QUANTITY = nameof(GroceryItem.InventoryQuantity);
    public const string FIELD_IS_OUT_OF_STOCK = "IsOutOfStock";
    public const string FIELD_IS_UNUSED = "IsUnused";
    public const string FIELD_STORAGE_LOCATION_IDS = nameof(StorageLocation) + nameof(StorageLocation.Id) + "s";
    public const string FIELD_STORAGE_LOCATIONS_JSON = nameof(GroceryItem.StorageLocations) + "Json";
    public const string FIELD_GROCERY_AISLE_ID = nameof(GroceryItem.GroceryAisleId);
    public const string FIELD_GROCERY_AISLE_JSON = nameof(GroceryItem.GroceryAisle) + "Json";
}
