namespace FoodStuffs.Model.Events.MealPlans;

public record GetMealPlanResponsePantryShoppingItem(
    int ShoppingItemId,
    string ShoppingItemName,
    decimal Quantity);
