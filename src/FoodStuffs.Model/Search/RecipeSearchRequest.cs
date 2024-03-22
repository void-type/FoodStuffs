using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search;

public record RecipeSearchRequest(
    string? NameSearch,
    int[]? CategoryIds,
    bool? IsForMealPlanning,
    string? SortBy,
    string? RandomSortSeed,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
