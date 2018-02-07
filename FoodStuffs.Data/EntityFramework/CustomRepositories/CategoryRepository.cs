using Core.Data.EntityFramework;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.EntityFramework.CustomRepositories
{
    public class CategoryRepository : EfWritableRepository<Category>
    {
        public override IQueryable<Category> Stored => Context.Set<Category>()
            .Include(c => c.CategoryRecipes)
            .ThenInclude(cr => cr.Recipe);

        public CategoryRepository(DbContext context) : base(context)
        {
        }
    }
}