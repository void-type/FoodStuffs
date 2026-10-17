using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.GroceryStores.Models;

public record ListGroceryStoresRequest(
    string? NameSearch,
    bool? IsUnused,
    string? SortBy,
    bool SortDescending,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
