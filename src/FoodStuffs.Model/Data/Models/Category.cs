using System.Collections.Generic;

namespace FoodStuffs.Model.Data.Models
{
    public class Category
    {
        public List<CategoryRecipe> CategoryRecipe { get; set; } = new List<CategoryRecipe>();
        public int Id { get; set; }
        public string Name { get; set; }
    }
}