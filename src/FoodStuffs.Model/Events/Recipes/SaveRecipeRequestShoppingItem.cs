namespace FoodStuffs.Model.Events.Recipes;

public record SaveRecipeRequestShoppingItem(
    int Id,
    int Quantity,
    int Order);
