namespace FoodStuffs.Model.Search;

public record RecipeSearchResultItem(
    int Id,
    string Name,
    bool IsForMealPlanning,
    DateTimeOffset CreatedOn,
    string Slug,
    List<string> Categories,
    List<RecipeSearchResultItemShoppingItem> ShoppingItems,
    string? Image);
