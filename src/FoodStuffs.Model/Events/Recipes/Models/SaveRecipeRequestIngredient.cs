namespace FoodStuffs.Model.Events.Recipes.Models;

public record SaveRecipeRequestIngredient(
    string Name,
    decimal Quantity,
    int Order,
    bool IsCategory);
