using System.Collections.Generic;

namespace FoodStuffs.Model.Interfaces.Domain
{
    public interface ICategory
    {
        ICollection<ICategoryRecipe> CategoryRecipe { get; set; }
        int Id { get; set; }
        string Name { get; set; }
    }
}