using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes.Models;

public record ListDeletedRecipesRequest(
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
