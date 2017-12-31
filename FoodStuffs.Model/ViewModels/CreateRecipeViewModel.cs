using System.Collections.Generic;

namespace FoodStuffs.Model.ViewModels
{
    public class CreateRecipeViewModel : ICreateRecipeViewModel
    {
        public IEnumerable<string> Categories { get; set; } = new List<string>();
        public int? CookTimeMinutes { get; set; }
        public string Directions { get; set; }
        public string Ingredients { get; set; }
        public string Name { get; set; }
        public int? PrepTimeMinutes { get; set; }
    }
}