using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Models;

public class Category : IAuditableWithOffset
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool ShowInMealPlan { get; set; }

    public string Color { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTimeOffset ModifiedOn { get; set; }

    public virtual List<Recipe> Recipes { get; } = [];
}
