namespace FoodStuffs.Model.Events.MealSets;

public record GetMealSetResponseRecipe(
    int Id,
    string Name,
    string? Image,
    IEnumerable<string> Categories,
    IEnumerable<GetMealSetResponseRecipeIngredient> Ingredients);
