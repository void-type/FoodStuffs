using System.Collections.Generic;

namespace FoodStuffs.Model.Data.Models;

public partial class Category
{
    public Category()
    {
        Recipes = new HashSet<Recipe>();
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; set; }
}
