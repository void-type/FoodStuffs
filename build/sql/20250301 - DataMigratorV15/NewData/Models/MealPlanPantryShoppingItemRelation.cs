namespace DataMigratorV15.NewData.Models;

public class MealPlanPantryShoppingItemRelation
{
    public int Quantity { get; set; }

    public int MealPlanId { get; set; }

    public int ShoppingItemId { get; set; }

    public virtual ShoppingItem ShoppingItem { get; set; } = null!;
}
