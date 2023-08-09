namespace FoodStuffs.Model.Events.MealSets;

public record GetMealSetResponseRecipeIngredient(
    string Name,
    int Quantity,
    int Order,
    bool IsCategory);
