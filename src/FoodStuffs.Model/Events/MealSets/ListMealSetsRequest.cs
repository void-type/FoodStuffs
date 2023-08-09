using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.MealSets;

public record ListMealSetsRequest(
    string? NameSearch,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
