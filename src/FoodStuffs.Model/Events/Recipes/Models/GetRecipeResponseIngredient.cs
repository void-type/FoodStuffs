namespace FoodStuffs.Model.Events.Recipes.Models;

public record GetRecipeResponseIngredient(
    string Name,
    decimal Quantity,
    int Order,
    bool IsCategory);
