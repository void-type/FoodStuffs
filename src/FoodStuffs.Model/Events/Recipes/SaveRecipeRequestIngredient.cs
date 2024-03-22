namespace FoodStuffs.Model.Events.Recipes;

public record SaveRecipeRequestIngredient(
    string Name,
    decimal Quantity,
    int Order,
    bool IsCategory);
