using System.Collections.Generic;

namespace FoodStuffs.Model.Data.Models
{
    public interface ICategory
    {
        ICollection<ICategoryRecipe> CategoryRecipes { get; set; }
        int Id { get; set; }
        string Name { get; set; }
    }
}