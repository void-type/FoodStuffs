namespace FoodStuffs.Model.Data.Models;

public class MealPlanPantryShoppingItemRelation
{
    public int Quantity { get; set; }

    public ShoppingItem ShoppingItem { get; set; } = null!;
}
