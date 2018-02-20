using Core.Data.EntityFramework;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.Service.CustomRepositories
{
    public class CategoryRecipeRepository : EfWritableRepository<CategoryRecipe>
    {
        public override IQueryable<CategoryRecipe> Stored => Context.Set<CategoryRecipe>()
            .Include(cr => cr.Category)
            .Include(cr => cr.Recipe);

        public CategoryRecipeRepository(DbContext context) : base(context)
        {
        }
    }
}