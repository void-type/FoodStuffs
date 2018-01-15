using FoodStuffs.Data.Models;
using FoodStuffs.Model.Interfaces.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Core.Data.EntityFramework;

namespace FoodStuffs.Data.CustomRepositories
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