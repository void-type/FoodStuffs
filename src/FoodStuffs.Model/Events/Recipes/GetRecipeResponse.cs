namespace FoodStuffs.Model.Events.Recipes;

public record GetRecipeResponse(
    int Id,
    string Name,
    string Directions,
    int? CookTimeMinutes,
    int? PrepTimeMinutes,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    string Slug,
    string? PinnedImage,
    bool IsForMealPlanning,
    IEnumerable<string> Categories,
    IEnumerable<string> Images,
    IEnumerable<GetRecipeResponseIngredient> Ingredients);
