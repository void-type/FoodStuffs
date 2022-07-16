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
    int? PinnedImageId,
    bool IsForMealPlanning,
    IEnumerable<string> Categories,
    IEnumerable<int> Images,
    IEnumerable<GetRecipeResponseIngredient> Ingredients);
