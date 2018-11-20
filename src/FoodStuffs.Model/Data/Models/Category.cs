using System.Collections.Generic;

namespace FoodStuffs.Model.Data.Models
{
    public partial class Category
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CategoryRecipe> CategoryRecipe { get; set; }

        public Category()
        {
            CategoryRecipe = new HashSet<CategoryRecipe>();
        }
    }
}
