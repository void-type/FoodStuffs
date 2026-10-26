namespace FoodStuffs.Model.Events.Recipes.Models;

public record ListDeletedRecipesResponse(
    int Id,
    string Name,
    DateTimeOffset? DeletedOn,
    string DeletedBy);
