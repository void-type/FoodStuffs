using System.Collections.Generic;

namespace FoodStuffs.Model.ViewModels
{
    public interface IRecipeListItem
    {
        IEnumerable<string> Categories { get; set; }
        int Id { get; set; }
        string Name { get; set; }
    }
}