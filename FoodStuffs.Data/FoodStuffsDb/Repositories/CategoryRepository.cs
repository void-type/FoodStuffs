using FoodStuffs.Data.FoodStuffsDb.Core;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Interfaces.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.FoodStuffsDb.Repositories
{
    public class CategoryRepository : EfRepository<ICategory, Category>
    {
        public CategoryRepository(DbContext context) : base(context)
        {
        }

        public new IQueryable<ICategory> Stored => Context.Set<Category>()
            .Include(c => c.CategoryRecipe)
            .AsQueryable();
    }
}