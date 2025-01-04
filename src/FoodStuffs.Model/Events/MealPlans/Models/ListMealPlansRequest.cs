using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.MealPlans.Models;

public record ListMealPlansRequest(
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
