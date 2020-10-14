using System.Collections.Generic;

#nullable disable
namespace FoodStuffs.Model.Data.Models
{
    public partial class Category
    {
        public Category()
        {
            CategoryRecipe = new HashSet<CategoryRecipe>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CategoryRecipe> CategoryRecipe { get; set; }
    }
}
