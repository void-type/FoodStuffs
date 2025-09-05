using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search.GroceryItems.Models;

public record SuggestGroceryItemsRequest(
    string? SearchText,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
