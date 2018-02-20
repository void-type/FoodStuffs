using System;
using System.Collections.Generic;

namespace FoodStuffs.Data
{
    public partial class CategoryRecipe
    {
        public int RecipeId { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public Recipe Recipe { get; set; }
    }
}
