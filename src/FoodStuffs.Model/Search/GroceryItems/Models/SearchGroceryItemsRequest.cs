using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search.GroceryItems.Models;

public record SearchGroceryItemsRequest(
    string? SearchText,
    int[]? StorageLocationIds,
    bool MatchAllStorageLocations,
    int[]? GroceryAisleIds,
    int[]? GroceryStoreIds,
    bool MatchAllGroceryStores,
    bool? IsOutOfStock,
    bool? IsUnused,
    string? SortBy,
    string? RandomSortSeed,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
