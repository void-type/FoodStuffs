using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.GroceryStores.Models;

public record ListGroceryStoresRequest(
    string? NameSearch,
    bool? IsUnused,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
