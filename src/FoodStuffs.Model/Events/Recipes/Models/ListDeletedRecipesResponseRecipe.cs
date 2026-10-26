namespace FoodStuffs.Model.Events.Recipes.Models;

/// <param name="PurgeOn">When the recipe will be permanently deleted automatically. Null when automatic deletion is off.</param>
public record ListDeletedRecipesResponseRecipe(
    int Id,
    string Name,
    DateTimeOffset? DeletedOn,
    string DeletedBy,
    DateTimeOffset? PurgeOn);
