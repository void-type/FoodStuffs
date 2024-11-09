using VoidCore.Model.Data;

namespace DataMigratorV14.NewData.Models;

public class MealPlan : IAuditableWithOffset
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTimeOffset ModifiedOn { get; set; }

    public virtual List<MealPlanPantryShoppingItemRelation> PantryShoppingItemRelations { get; } = [];

    public virtual List<MealPlanRecipeRelation> RecipeRelations { get; } = [];
}
