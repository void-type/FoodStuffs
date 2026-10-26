namespace FoodStuffs.Model.Events.Recipes.Models;

// Events take a request object even when there is nothing to pass.
#pragma warning disable S2094 // Classes should not be empty
public record PurgeExpiredRecipesRequest;
#pragma warning restore S2094 // Classes should not be empty
