namespace FoodStuffs.Model.Search;

public record RecipeSearchResultItem(
    int Id,
    string Name,
    bool IsForMealPlanning,
    DateTimeOffset CreatedOn,
    string Slug,
    IEnumerable<string> Categories,
    IEnumerable<RecipeSearchResultItemIngredient> Ingredients,
    string? Image);
