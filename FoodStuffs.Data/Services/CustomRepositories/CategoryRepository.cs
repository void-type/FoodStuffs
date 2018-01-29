using Core.Data.EntityFramework;
using FoodStuffs.Data.Models;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.Services.CustomRepositories
{
    public class CategoryRepository : EfWritableRepository<ICategory, Category>
    {
        public override IQueryable<ICategory> Stored => Context.Set<Category>()
            .Include(c => c.CategoryRecipe)
            .ThenInclude(cr => cr.Recipe);

        public CategoryRepository(DbContext context) : base(context)
        {
        }
    }
}