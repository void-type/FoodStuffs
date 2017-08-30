using FoodStuffs.Data.FoodStuffsDb.Core;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Interfaces.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.FoodStuffsDb.Repositories
{
    public class RecipeRepository : EfRepository<IRecipe, Recipe>
    {
        public RecipeRepository(DbContext context) : base(context)
        {
        }

        public new IQueryable<IRecipe> Stored => Context.Set<Recipe>()
            .Include(r => r.CategoryRecipe)
            .Include(r => r.CreatedByUser)
            .Include(r => r.ModifiedByUser)
            .AsQueryable();
    }
}