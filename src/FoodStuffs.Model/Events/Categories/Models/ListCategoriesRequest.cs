using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Categories.Models;

public record ListCategoriesRequest(
    string? NameSearch,
    bool? IsUnused,
    string? SortBy,
    bool SortDescending,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
