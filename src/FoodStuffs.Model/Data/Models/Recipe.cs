using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.Data.Models
{
    public partial class Recipe
    {
        public Recipe()
        {
            CategoryRecipe = new HashSet<CategoryRecipe>();
            Image = new HashSet<Image>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Directions { get; set; }
        public string Ingredients { get; set; }
        public int? PrepTimeMinutes { get; set; }
        public int? CookTimeMinutes { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual ICollection<CategoryRecipe> CategoryRecipe { get; set; }
        public virtual ICollection<Image> Image { get; set; }
    }
}
