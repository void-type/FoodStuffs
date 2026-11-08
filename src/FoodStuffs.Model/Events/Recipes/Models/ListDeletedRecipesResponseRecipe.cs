namespace FoodStuffs.Model.Events.Recipes.Models;

/// <summary>
/// A recipe in the recycle bin.
/// </summary>
/// <param name="Id">The recipe ID.</param>
/// <param name="Name">The recipe name.</param>
/// <param name="DeletedOn">When the recipe was moved to the recycle bin.</param>
/// <param name="DeletedBy">Who moved the recipe to the recycle bin.</param>
/// <param name="PurgeOn">When the recipe will be permanently deleted automatically. Null when automatic deletion is off.</param>
public record ListDeletedRecipesResponseRecipe(
    int Id,
    string Name,
    DateTimeOffset? DeletedOn,
    string DeletedBy,
    DateTimeOffset? PurgeOn);
