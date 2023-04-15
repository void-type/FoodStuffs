namespace FoodStuffs.Model.Events.Recipes;

public record ListRecipesResponseIngredient(
    string Name,
    int Quantity,
    int Order,
    bool IsCategory);
