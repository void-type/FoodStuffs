namespace FoodStuffs.Model.Events.MealSets;

public record GetMealSetResponseRecipe(
    int Id,
    string Name,
    int? PinnedImageId,
    IEnumerable<string> Categories,
    IEnumerable<int> Images,
    IEnumerable<GetMealSetResponseRecipeIngredient> Ingredients);
