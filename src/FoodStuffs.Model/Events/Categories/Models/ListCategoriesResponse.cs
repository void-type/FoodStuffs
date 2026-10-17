namespace FoodStuffs.Model.Events.Categories.Models;

public record ListCategoriesResponse(
    int Id,
    string Name,
    string Color,
    bool ShowOnCards,
    int RecipeCount);
