namespace DataMigratorV15.NewData.Models;

public class MealPlanRecipeRelation
{
    public int Order { get; set; }

    public int MealPlanId { get; set; }

    public int RecipeId { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;
}
