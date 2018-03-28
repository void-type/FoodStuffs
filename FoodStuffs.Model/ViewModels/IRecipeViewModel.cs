using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.ViewModels
{
    public interface IRecipeViewModel
    {
        IEnumerable<string> Categories { get; set; }
        int? CookTimeMinutes { get; set; }
        string CreatedBy { get; set; }
        DateTime CreatedOnUtc { get; set; }
        string Directions { get; set; }
        int Id { get; set; }
        string Ingredients { get; set; }
        string ModifiedBy { get; set; }
        DateTime ModifiedOnUtc { get; set; }
        string Name { get; set; }
        int? PrepTimeMinutes { get; set; }
    }
}