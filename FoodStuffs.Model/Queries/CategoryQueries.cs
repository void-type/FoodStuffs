using FoodStuffs.Model.Interfaces.Domain;
using System.Linq;

namespace FoodStuffs.Model.Queries
{
    public static class CategoryQueries
    {
        public static ICategory GetByName(this IQueryable<ICategory> categories, string categoryName)
        {
            return categories.FirstOrDefault(c => c.Name.ToUpper().Trim() == categoryName.ToUpper().Trim());
        }

        public static ICategory GetById(this IQueryable<ICategory> categories, int id)
        {
            return categories.SingleOrDefault(r => r.Id == id);
        }
    }
}