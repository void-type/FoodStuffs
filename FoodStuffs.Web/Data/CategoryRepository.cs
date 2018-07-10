using Core.Services.Data;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Web.Data
{
    public class CategoryRepository : EfWritableRepository<Category>
    {
        public override IQueryable<Category> Stored => Context.Set<Category>()
        .Include(c => c.CategoryRecipe)
        .ThenInclude(cr => cr.Recipe);

        public CategoryRepository(DbContext context) : base(context)
        { }
    }
}
