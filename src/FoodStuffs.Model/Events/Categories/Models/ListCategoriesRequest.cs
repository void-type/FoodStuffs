using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Categories.Models;

public record ListCategoriesRequest(
    string? NameSearch,
    bool? IsUnused,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
