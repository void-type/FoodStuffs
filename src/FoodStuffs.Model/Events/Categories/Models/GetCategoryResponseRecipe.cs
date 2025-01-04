namespace FoodStuffs.Model.Events.Categories.Models;

public record GetCategoryResponseRecipe(
    int Id,
    string Name,
    string? Image);
