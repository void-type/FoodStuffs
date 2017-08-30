using FoodStuffs.Model.Interfaces.Domain;
using System.Linq;

namespace FoodStuffs.Model.Queries
{
    public static class RecipeQueries
    {
        public static IRecipe GetById(this IQueryable<IRecipe> recipes, int id)
        {
            return recipes.FirstOrDefault(r => r.Id == id);
        }
    }
}