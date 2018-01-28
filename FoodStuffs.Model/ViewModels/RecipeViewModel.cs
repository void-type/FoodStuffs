using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.ViewModels
{
    public class RecipeViewModel : IRecipeViewModel
    {
        public IEnumerable<string> Categories { get; set; } = new List<string>();
        public int? CookTimeMinutes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Directions { get; set; }
        public int Id { get; set; }
        public string Ingredients { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Name { get; set; }
        public int? PrepTimeMinutes { get; set; }
    }
}