using FoodStuffs.Model.Interfaces.Domain;
using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.ViewModels
{
    public class RecipeViewModel
    {
        public List<ICategory> Categories { get; set; } = new List<ICategory>();
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