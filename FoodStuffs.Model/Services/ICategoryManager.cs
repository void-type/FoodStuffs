using System.Collections.Generic;
using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Services
{
    public interface ICategoryManager
    {
         IEnumerable<Category> FindUnusedCategories(Recipe recipe, IEnumerable<CategoryRecipe> categoryRecipes);
    }
}
