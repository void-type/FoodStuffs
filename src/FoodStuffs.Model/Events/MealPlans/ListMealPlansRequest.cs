using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.MealPlans;

public record ListMealPlansRequest(
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
