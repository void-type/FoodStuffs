using System;
using System.Collections.Generic;

namespace FoodStuffs.Data
{
    public partial class Category
    {
        public Category()
        {
            CategoryRecipe = new HashSet<CategoryRecipe>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CategoryRecipe> CategoryRecipe { get; set; }
    }
}
