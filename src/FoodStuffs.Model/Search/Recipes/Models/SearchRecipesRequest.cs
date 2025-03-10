using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search.Recipes.Models;

public record SearchRecipesRequest(
    string? SearchText,
    int[]? CategoryIds,
    bool AllCategories,
    bool? IsForMealPlanning,
    string? SortBy,
    string? RandomSortSeed,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
