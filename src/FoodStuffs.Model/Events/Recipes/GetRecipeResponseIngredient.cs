namespace FoodStuffs.Model.Events.Recipes;

public record GetRecipeResponseIngredient(
    string Name,
    decimal Quantity,
    int Order,
    bool IsCategory);
