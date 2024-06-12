namespace FoodStuffs.Model.Data.Models;

public class MealPlanPantryShoppingItemRelation
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int MealPlanId { get; set; }

    public int ShoppingItemId { get; set; }

    public virtual ShoppingItem ShoppingItem { get; set; } = null!;
}
