using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.ViewModels
{
    public class RecipeViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public int? CookTimeMinutes { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Directions { get; set; }
        public string Ingredients { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int? PrepTimeMinutes { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}