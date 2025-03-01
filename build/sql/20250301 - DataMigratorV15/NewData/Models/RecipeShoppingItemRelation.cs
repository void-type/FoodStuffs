namespace DataMigratorV15.NewData.Models;

public class RecipeShoppingItemRelation
{
    public int Quantity { get; set; }

    public int Order { get; set; }

    public int RecipeId { get; set; }

    public int ShoppingItemId { get; set; }

    public virtual ShoppingItem ShoppingItem { get; set; } = null!;
}
