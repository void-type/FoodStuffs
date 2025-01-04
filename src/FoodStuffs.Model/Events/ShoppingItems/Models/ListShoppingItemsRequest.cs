using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.ShoppingItems.Models;

public record ListShoppingItemsRequest(
    string? NameSearch,
    bool? IsUnused,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
