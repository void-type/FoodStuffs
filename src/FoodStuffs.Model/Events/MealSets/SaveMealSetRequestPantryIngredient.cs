namespace FoodStuffs.Model.Events.MealSets;

public record SaveMealSetRequestPantryIngredient(
    string Name,
    decimal Quantity);
