namespace FoodStuffs.Model.Search.Recipes.Models;

public record SuggestRecipesResultItem(
    int Id,
    string Name,
    string Slug,
    string? Image);
