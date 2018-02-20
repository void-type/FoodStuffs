using System;
using System.Collections.Generic;

namespace FoodStuffs.Data
{
    public partial class Recipe
    {
        public Recipe()
        {
            CategoryRecipe = new HashSet<CategoryRecipe>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Directions { get; set; }
        public string Ingredients { get; set; }
        public int? PrepTimeMinutes { get; set; }
        public int? CookTimeMinutes { get; set; }
        public int CreatedByUserId { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public User CreatedByUser { get; set; }
        public User ModifiedByUser { get; set; }
        public ICollection<CategoryRecipe> CategoryRecipe { get; set; }
    }
}
