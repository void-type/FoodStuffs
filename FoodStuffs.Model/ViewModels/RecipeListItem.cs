using System.Collections.Generic;

namespace FoodStuffs.Model.ViewModels
{
    public class RecipeListItem : IRecipeListItem
    {
        public IEnumerable<string> Categories { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}