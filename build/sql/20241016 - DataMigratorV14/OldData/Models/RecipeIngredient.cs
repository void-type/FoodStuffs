using VoidCore.Model.Data;

namespace DataMigratorV14.OldData.Models;

public class RecipeIngredient
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Quantity { get; set; }

    public int Order { get; set; }

    public bool IsCategory { get; set; }
}
