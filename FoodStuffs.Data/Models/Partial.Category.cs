using FoodStuffs.Model.Interfaces.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Data.Models
{
    public partial class Category : ICategory
    {
        ICollection<ICategoryRecipe> ICategory.CategoryRecipes
        {
            get => CategoryRecipe.Select(x => (ICategoryRecipe)x).ToArray();
            set => CategoryRecipe = value.Select(x => (CategoryRecipe)x).ToArray();
        }
    }
}