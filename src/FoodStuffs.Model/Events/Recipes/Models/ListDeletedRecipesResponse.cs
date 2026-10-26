using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes.Models;

/// <param name="RetentionDays">Days recipes stay in the recycle bin before they are permanently deleted automatically. Null when automatic deletion is off.</param>
/// <param name="Recipes">The deleted recipes</param>
public record ListDeletedRecipesResponse(
    int? RetentionDays,
    IItemSet<ListDeletedRecipesResponseRecipe> Recipes);
