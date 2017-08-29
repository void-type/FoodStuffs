using System.Collections.Generic;

namespace FoodStuffs.Model.Interfaces.Domain
{
    public interface ICategory
    {
        int Id { get; set; }
        string Name { get; set; }
        ICollection<ICategoryRecipe> CategoryRecipe { get; set; }
    }
}