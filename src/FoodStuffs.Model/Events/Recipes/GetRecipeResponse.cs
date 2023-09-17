namespace FoodStuffs.Model.Events.Recipes;

public record GetRecipeResponse(
    int Id,
    string Name,
    string Directions,
    int? CookTimeMinutes,
    int? PrepTimeMinutes,
    string CreatedBy,
    DateTime CreatedOn,
    string ModifiedBy,
    DateTime ModifiedOn,
    string? PinnedImage,
    bool IsForMealPlanning,
    IEnumerable<string> Categories,
    IEnumerable<string> Images,
    IEnumerable<GetRecipeResponseIngredient> Ingredients);
