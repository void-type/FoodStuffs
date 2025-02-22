using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Models;

public class ShoppingItem : IAuditableWithOffset
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int InventoryQuantity { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTimeOffset ModifiedOn { get; set; }

    public int? GroceryDepartmentId { get; set; }

    public virtual GroceryDepartment? GroceryDepartment { get; set; }

    public virtual List<PantryLocation> PantryLocations { get; } = [];

    public virtual List<Recipe> Recipes { get; } = [];
}
