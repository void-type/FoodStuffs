namespace FoodStuffs.Model.Events.MealSets;

public record GetMealSetResponseRecipeIngredient(
    string Name,
    decimal Quantity,
    int Order,
    bool IsCategory);
