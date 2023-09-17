namespace FoodStuffs.Model.Events.MealSets;

public record GetMealSetResponse(
    int Id,
    string Name,
    string CreatedBy,
    DateTime CreatedOn,
    string ModifiedBy,
    DateTime ModifiedOn,
    IEnumerable<GetMealSetResponsePantryIngredient> PantryIngredients,
    IEnumerable<GetMealSetResponseRecipe> Recipes);
