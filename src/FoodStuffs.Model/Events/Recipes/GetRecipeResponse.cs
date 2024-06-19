namespace FoodStuffs.Model.Events.Recipes;

public record GetRecipeResponse(
    int Id,
    string Name,
    string Directions,
    int? PrepTimeMinutes,
    int? CookTimeMinutes,
    bool IsForMealPlanning,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    string Slug,
    string? DefaultImage,
    string? PinnedImage,
    List<string> Images,
    List<string> Categories,
    List<GetRecipeResponseIngredient> Ingredients,
    List<GetRecipeResponseShoppingItem> ShoppingItems);
