using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.GroceryDepartments.Models;

public record ListGroceryDepartmentsRequest(
    string? NameSearch,
    bool? IsUnused,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
