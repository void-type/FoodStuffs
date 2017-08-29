using System.Collections.Generic;

namespace FoodStuffs.Data.FoodStuffsDb.Models
{
    public partial class Recipe
    {
        public virtual ICollection<CategoryRecipe> CategoryRecipe { get; set; }

        public int? CookTimeMinutes { get; set; }

        public virtual User CreatedByUser { get; set; }

        public int CreatedByUserId { get; set; }

        public System.DateTime CreatedOn { get; set; }

        public string Directions { get; set; }

        public int Id { get; set; }

        public string Ingredients { get; set; }

        public virtual User ModifiedByUser { get; set; }

        public int ModifiedByUserId { get; set; }

        public System.DateTime ModifiedOn { get; set; }

        public string Name { get; set; }

        public int? PrepTimeMinutes { get; set; }

        public Recipe()
        {
            CategoryRecipe = new HashSet<CategoryRecipe>();
        }
    }
}