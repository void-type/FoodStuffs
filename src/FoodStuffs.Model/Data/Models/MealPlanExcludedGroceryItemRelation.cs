namespace FoodStuffs.Model.Data.Models;

public class MealPlanExcludedGroceryItemRelation
{
    public int Quantity { get; set; }

    public int MealPlanId { get; set; }

    public int GroceryItemId { get; set; }

    public virtual GroceryItem GroceryItem { get; set; } = null!;
}
