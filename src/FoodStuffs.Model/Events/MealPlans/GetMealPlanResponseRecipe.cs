namespace FoodStuffs.Model.Events.MealPlans;

public record GetMealPlanResponseRecipe(
    int Id,
    string Name,
    string? Image,
    IEnumerable<string> Categories,
    IEnumerable<GetMealPlanResponseRecipeIngredient> Ingredients);
