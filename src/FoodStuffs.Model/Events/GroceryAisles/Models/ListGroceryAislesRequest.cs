using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.GroceryAisles.Models;

public record ListGroceryAislesRequest(
    string? NameSearch,
    bool? IsUnused,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
