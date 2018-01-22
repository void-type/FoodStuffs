using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.ViewModels
{
    public interface IRecipeViewModel
    {
        IEnumerable<string> Categories { get; set; }
        int? CookTimeMinutes { get; set; }
        int CreatedByUserId { get; set; }
        DateTime CreatedOn { get; set; }
        string Directions { get; set; }
        int Id { get; set; }
        string Ingredients { get; set; }
        int ModifiedByUserId { get; set; }
        DateTime ModifiedOn { get; set; }
        string Name { get; set; }
        int? PrepTimeMinutes { get; set; }
    }
}