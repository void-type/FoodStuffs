using FoodStuffs.Data.FoodStuffsDb.Core;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Interfaces.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.FoodStuffsDb.Repositories
{
    public class RecipeRepository : Repository<IRecipe, Recipe>
    {
        public RecipeRepository(DbContext context) : base(context)
        {
        }

        public override IQueryable<IRecipe> Stored => Context.Set<Recipe>()
            .Include(r => r.CategoryRecipe)
            .ThenInclude(cr => cr.Category)
            .Include(r => r.CreatedByUser)
            .Include(r => r.ModifiedByUser);
    }
}