using System.Collections.Generic;

namespace FoodStuffs.Model.Events.Recipes;

public record SaveRecipeRequest(
    int Id,
    string Name,
    string Directions,
    int? CookTimeMinutes,
    int? PrepTimeMinutes,
    IEnumerable<SaveRecipeRequestIngredient> Ingredients,
    IEnumerable<string> Categories);
