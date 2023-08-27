namespace FoodStuffs.Model.Events.MealSets;

public record GetMealSetResponse(
    int Id,
    string Name,
    string CreatedBy,
    DateTime CreatedOn,
    string ModifiedBy,
    DateTime ModifiedOn,
    IEnumerable<GetMealSetResponseRecipe> Recipes,
    IEnumerable<GetMealSetResponsePantryIngredient> PantryIngredients);
