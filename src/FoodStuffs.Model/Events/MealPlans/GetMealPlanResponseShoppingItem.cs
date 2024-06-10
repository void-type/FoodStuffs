namespace FoodStuffs.Model.Events.MealPlans;

public record GetMealPlanResponseShoppingItem(
    int ShoppingItemId,
    string ShoppingItemName,
    decimal Quantity);
