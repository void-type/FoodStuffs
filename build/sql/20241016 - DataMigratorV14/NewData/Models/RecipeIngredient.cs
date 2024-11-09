namespace DataMigratorV14.NewData.Models;

public class RecipeIngredient
{
    public string Name { get; set; } = null!;

    public decimal Quantity { get; set; }

    public int Order { get; set; }

    public bool IsCategory { get; set; }
}
