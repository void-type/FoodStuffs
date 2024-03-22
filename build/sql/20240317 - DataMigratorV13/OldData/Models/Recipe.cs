namespace DataMigratorV13.OldData.Models;

public partial class Recipe
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Directions { get; set; } = null!;

    public int? PrepTimeMinutes { get; set; }

    public int? CookTimeMinutes { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime ModifiedOn { get; set; }

    public int? PinnedImageId { get; set; }

    public bool IsForMealPlanning { get; set; }

    public virtual List<Image> Images { get; } = [];

    public virtual List<Ingredient> Ingredients { get; } = [];

    public virtual Image? PinnedImage { get; set; }

    public virtual List<Category> Categories { get; } = [];

    public virtual List<MealSet> MealSets { get; } = [];
}
