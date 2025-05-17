using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.GroceryItems.Models;

public record ListGroceryItemsRequest(
    string? NameSearch,
    bool? IsUnused,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
