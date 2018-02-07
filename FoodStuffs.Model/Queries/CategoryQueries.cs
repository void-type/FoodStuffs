using FoodStuffs.Model.Data.Models;
using System.Linq;

namespace FoodStuffs.Model.Queries
{
    public static class CategoryQueries
    {
        public static Category GetById(this IQueryable<Category> categories, int id)
        {
            return categories.SingleOrDefault(r => r.Id == id);
        }

        public static Category GetByName(this IQueryable<Category> categories, string categoryName)
        {
            return categories.FirstOrDefault(c => c.Name.ToUpper().Trim() == categoryName.ToUpper().Trim());
        }
    }
}