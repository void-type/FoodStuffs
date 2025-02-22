namespace FoodStuffs.Model.Events.MealPlans.Models;

public record GetMealPlanResponse(
    int Id,
    string Name,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    List<GetMealPlanResponseExcludedShoppingItem> ExcludedShoppingItems,
    List<GetMealPlanResponseRecipe> Recipes);
