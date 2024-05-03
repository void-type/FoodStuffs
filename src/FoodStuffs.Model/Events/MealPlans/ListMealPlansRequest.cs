using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.MealPlans;

public record ListMealPlansRequest(
    string? NameSearch,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
