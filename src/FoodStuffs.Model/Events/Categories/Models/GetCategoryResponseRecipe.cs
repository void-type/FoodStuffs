namespace FoodStuffs.Model.Events.Categories.Models;

public record GetCategoryResponseRecipe(
    int Id,
    string Name,
    string Slug,
    string? Image);
