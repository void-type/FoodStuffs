using FoodStuffs.Data.FoodStuffsDb.Core;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Interfaces.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.FoodStuffsDb.Repositories
{
    public class CategoryRecipeRepository : EfRepository<ICategoryRecipe, CategoryRecipe>
    {
        public CategoryRecipeRepository(DbContext context) : base(context)
        {
        }

        public new IQueryable<ICategoryRecipe> Stored => Context.Set<CategoryRecipe>()
            .Include(cr => cr.Category)
            .Include(cr => cr.Recipe)
            .AsQueryable();
    }
}