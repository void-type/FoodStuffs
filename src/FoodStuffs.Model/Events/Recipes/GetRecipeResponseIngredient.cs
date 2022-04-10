namespace FoodStuffs.Model.Events.Recipes;

public record GetRecipeResponseIngredient(
    string Name,
    int Quantity,
    int Order,
    bool IsCategory);
