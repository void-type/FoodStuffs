using System.Collections.Generic;

namespace FoodStuffs.Model.Interfaces.Services.Data.Models
{
    public interface ICategory
    {
        ICollection<ICategoryRecipe> CategoryRecipe { get; set; }
        int Id { get; set; }
        string Name { get; set; }
    }
}