namespace FoodStuffs.Model.Search.Recipes.Models;

public record SearchRecipesResultItem(
    int Id,
    string Name,
    bool IsForMealPlanning,
    DateTimeOffset CreatedOn,
    string Slug,
    List<string> Categories,
    List<SearchRecipesResultItemShoppingItem> ShoppingItems,
    string? Image);
