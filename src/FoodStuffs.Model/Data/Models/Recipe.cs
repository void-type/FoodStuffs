using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.Data.Models;

public partial class Recipe
{
    public Recipe()
    {
        Images = new HashSet<Image>();
        Ingredients = new HashSet<Ingredient>();
        Categories = new HashSet<Category>();
    }

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

    public virtual Image? PinnedImage { get; set; }
    public virtual ICollection<Image> Images { get; set; }
    public virtual ICollection<Ingredient> Ingredients { get; set; }

    public virtual ICollection<Category> Categories { get; set; }
}
