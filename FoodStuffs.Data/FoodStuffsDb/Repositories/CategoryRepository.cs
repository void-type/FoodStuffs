using FoodStuffs.Data.FoodStuffsDb.Core;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Interfaces.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.FoodStuffsDb.Repositories
{
    public class CategoryRepository : Repository<ICategory, Category>
    {
        public CategoryRepository(DbContext context) : base(context)
        {
        }

        public override IQueryable<ICategory> Stored => Context.Set<Category>()
            .Include(c => c.CategoryRecipe)
            .ThenInclude(cr => cr.Recipe);
    }
}