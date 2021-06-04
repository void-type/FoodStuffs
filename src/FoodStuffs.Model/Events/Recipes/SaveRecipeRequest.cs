using System.Collections.Generic;

namespace FoodStuffs.Model.Events.Recipes
{
    public record SaveRecipeRequest(
        int Id,
        string Name,
        string Ingredients,
        string Directions,
        int? CookTimeMinutes,
        int? PrepTimeMinutes,
        IEnumerable<string> Categories);
}
