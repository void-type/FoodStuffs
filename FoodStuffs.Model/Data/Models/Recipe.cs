using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.Data.Models
{
    public class Recipe
    {
        public ICollection<CategoryRecipe> CategoryRecipe { get; set; }

        public int? CookTimeMinutes { get; set; }

        public User CreatedByUser { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public string Directions { get; set; }

        public int Id { get; set; }

        public string Ingredients { get; set; }

        public User ModifiedByUser { get; set; }

        public int ModifiedByUserId { get; set; }

        public DateTime ModifiedOnUtc { get; set; }

        public string Name { get; set; }

        public int? PrepTimeMinutes { get; set; }

        public Recipe()
        {
            CategoryRecipe = new HashSet<CategoryRecipe>();
        }
    }
}