namespace FoodStuffs.Model.Data.Models;

public class RecipeShoppingItemRelation
{
    public int Quantity { get; set; }

    public int Order { get; set; }

    public ShoppingItem ShoppingItem { get; set; } = null!;
}
