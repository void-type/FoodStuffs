using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.GroceryAisles.Models;

public record ListGroceryAislesRequest(
    string? NameSearch,
    bool? IsUnused,
    string? SortBy,
    bool SortDescending,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
