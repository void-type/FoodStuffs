namespace FoodStuffs.Model.Events.Categories.Models;

public record ListCategoriesResponse(
    int Id,
    string Name,
    string Color,
    int RecipeCount);
