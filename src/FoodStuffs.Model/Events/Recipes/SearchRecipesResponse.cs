namespace FoodStuffs.Model.Events.Recipes;

public record SearchRecipesResponse(
    int Id,
    string Name,
    bool IsForMealPlanning,
    DateTime CreatedOn,
    IEnumerable<string> Categories,
    IEnumerable<SearchRecipesResponseIngredient> Ingredients,
    string? Image);
