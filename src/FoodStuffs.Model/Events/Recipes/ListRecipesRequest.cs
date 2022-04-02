using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

public record ListRecipesRequest(
    string? NameSearch,
    string? CategorySearch,
    string? SortBy,
    bool SortDesc,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
