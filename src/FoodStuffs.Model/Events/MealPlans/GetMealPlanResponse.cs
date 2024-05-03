namespace FoodStuffs.Model.Events.MealPlans;

public record GetMealPlanResponse(
    int Id,
    string Name,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    IEnumerable<GetMealPlanResponsePantryIngredient> PantryIngredients,
    IEnumerable<GetMealPlanResponseRecipe> Recipes);
