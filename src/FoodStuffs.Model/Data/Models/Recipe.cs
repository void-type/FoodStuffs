using System;
using System.Collections.Generic;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Models
{
    public class Recipe : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Directions { get; set; }
        public string Ingredients { get; set; }
        public int? PrepTimeMinutes { get; set; }
        public int? CookTimeMinutes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        
        public ICollection<CategoryRecipe> CategoryRecipe { get; set; }

        public Recipe()
        {
            CategoryRecipe = new HashSet<CategoryRecipe>();
        }
    }
}
