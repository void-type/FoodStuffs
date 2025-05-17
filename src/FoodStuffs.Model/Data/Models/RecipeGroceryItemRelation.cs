namespace FoodStuffs.Model.Data.Models;

public class RecipeGroceryItemRelation
{
    public int Quantity { get; set; }

    public int Order { get; set; }

    public int RecipeId { get; set; }

    public int GroceryItemId { get; set; }

    public virtual GroceryItem GroceryItem { get; set; } = null!;
}
