using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.ShoppingItems;

public record ListShoppingItemsRequest(
    string? NameSearch,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
