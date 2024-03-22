namespace FoodStuffs.Model.Events.MealSets;

public record GetMealSetResponse(
    int Id,
    string Name,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    IEnumerable<GetMealSetResponsePantryIngredient> PantryIngredients,
    IEnumerable<GetMealSetResponseRecipe> Recipes);
