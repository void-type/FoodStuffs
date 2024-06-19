namespace FoodStuffs.Model.Events.Recipes;

public record SaveRecipeRequest(
    int Id,
    string Name,
    string Directions,
    int? CookTimeMinutes,
    int? PrepTimeMinutes,
    bool IsForMealPlanning,
    List<SaveRecipeRequestIngredient> Ingredients,
    List<SaveRecipeRequestShoppingItem> ShoppingItems,
    List<string> Categories);
