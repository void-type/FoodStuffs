using System.Collections.Generic;

namespace FoodStuffs.Data.Models
{
    public partial class Category
    {
        public virtual ICollection<CategoryRecipe> CategoryRecipe { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public Category()
        {
            CategoryRecipe = new HashSet<CategoryRecipe>();
        }
    }
}