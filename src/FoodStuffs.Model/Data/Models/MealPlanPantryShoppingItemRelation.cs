namespace FoodStuffs.Model.Data.Models;

public class MealPlanPantryShoppingItemRelation
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public ShoppingItem ShoppingItem { get; set; } = null!;
}
