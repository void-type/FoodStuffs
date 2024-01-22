namespace FoodStuffs.Model.Events.Recipes;

public record SearchRecipesResponseIngredient(
    string Name,
    int Quantity,
    int Order,
    bool IsCategory);
