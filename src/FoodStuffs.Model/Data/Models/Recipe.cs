using System;
using System.Collections.Generic;

#nullable disable

namespace FoodStuffs.Model.Data.Models;

public partial class Recipe
{
    public Recipe()
    {
        CategoryRecipes = new HashSet<CategoryRecipe>();
        Images = new HashSet<Image>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Directions { get; set; }
    public string Ingredients { get; set; }
    public int? PrepTimeMinutes { get; set; }
    public int? CookTimeMinutes { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
    public int? PinnedImageId { get; set; }

    public virtual Image PinnedImage { get; set; }
    public virtual ICollection<CategoryRecipe> CategoryRecipes { get; set; }
    public virtual ICollection<Image> Images { get; set; }
}
