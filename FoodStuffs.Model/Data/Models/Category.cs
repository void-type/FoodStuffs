using System.Collections.Generic;

namespace FoodStuffs.Model.Data.Models
{
    public partial class Category
    {
        public virtual ICollection<CategoryRecipe> CategoryRecipes { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public Category()
        {
            CategoryRecipes = new HashSet<CategoryRecipe>();
        }
    }
}