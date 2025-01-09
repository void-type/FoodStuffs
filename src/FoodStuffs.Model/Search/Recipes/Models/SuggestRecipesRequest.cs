using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search.Recipes.Models;

public record SuggestRecipesRequest(
    string? SearchText,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
