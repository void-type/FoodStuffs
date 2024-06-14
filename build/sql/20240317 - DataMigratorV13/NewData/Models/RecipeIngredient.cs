using VoidCore.Model.Data;

namespace DataMigratorV13.NewData.Models;

public class RecipeIngredient : IAuditableWithOffset
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Quantity { get; set; }

    public int Order { get; set; }

    public bool IsCategory { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTimeOffset ModifiedOn { get; set; }
}
