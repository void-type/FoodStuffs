namespace FoodStuffs.Model.Events.Recipes;

public record SaveRecipeRequestIngredient(
    string Name,
    int Quantity,
    int Order,
    bool IsCategory);
