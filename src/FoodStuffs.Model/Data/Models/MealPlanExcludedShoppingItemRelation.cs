namespace FoodStuffs.Model.Data.Models;

public class MealPlanExcludedShoppingItemRelation
{
    public int Quantity { get; set; }

    public int MealPlanId { get; set; }

    public int ShoppingItemId { get; set; }

    public virtual ShoppingItem ShoppingItem { get; set; } = null!;
}
