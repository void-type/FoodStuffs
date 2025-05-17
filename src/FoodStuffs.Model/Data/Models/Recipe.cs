using VoidCore.Model.Data;
using VoidCore.Model.Text;

namespace FoodStuffs.Model.Data.Models;

public class Recipe : IAuditableWithOffset
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Directions { get; set; } = null!;

    public string Sides { get; set; } = null!;

    public int? PrepTimeMinutes { get; set; }

    public int? CookTimeMinutes { get; set; }

    public bool IsForMealPlanning { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTimeOffset ModifiedOn { get; set; }

    public string Slug => Name.Slugify(230, true);

    public Image? DefaultImage => PinnedImage ?? Images.FirstOrDefault();

    public int? PinnedImageId { get; set; }

    public virtual Image? PinnedImage { get; set; }

    public virtual List<Image> Images { get; } = [];

    public virtual List<Category> Categories { get; } = [];

    public virtual List<RecipeGroceryItemRelation> GroceryItemRelations { get; } = [];

    public virtual List<MealPlanRecipeRelation> MealPlanRelations { get; } = [];
}
