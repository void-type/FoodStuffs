namespace FoodStuffs.Model.Events.Recipes.Models;

public record GetRecipeResponse(
    int Id,
    string Name,
    string Directions,
    string Sides,
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
    List<GetRecipeResponseShoppingItem> ShoppingItems);
