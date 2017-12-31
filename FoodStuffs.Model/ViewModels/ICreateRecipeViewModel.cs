using System.Collections.Generic;

namespace FoodStuffs.Model.ViewModels
{
    public interface ICreateRecipeViewModel
    {
        IEnumerable<string> Categories { get; set; }
        int? CookTimeMinutes { get; set; }
        string Directions { get; set; }
        string Ingredients { get; set; }
        string Name { get; set; }
        int? PrepTimeMinutes { get; set; }
    }
}