namespace FoodStuffs.Model.Search;

public record RecipeSearchResultItemIngredient(
    string Name,
    decimal Quantity,
    int Order,
    bool IsCategory);
