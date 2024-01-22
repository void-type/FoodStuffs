using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

public record SearchRecipesRequest(
    string? NameSearch,
    int[]? CategoryIds,
    bool? IsForMealPlanning,
    string? SortBy,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
