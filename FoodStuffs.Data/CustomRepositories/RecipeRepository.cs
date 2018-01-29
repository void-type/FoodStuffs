using Core.Data.EntityFramework;
using FoodStuffs.Data.Models;
using FoodStuffs.Model.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.CustomRepositories
{
    public class RecipeRepository : EfWritableRepository<IRecipe, Recipe>
    {
        public override IQueryable<IRecipe> Stored => Context.Set<Recipe>()
            .Include(r => r.CategoryRecipe)
            .ThenInclude(cr => cr.Category)
            .Include(r => r.CreatedByUser)
            .Include(r => r.ModifiedByUser);

        public RecipeRepository(DbContext context) : base(context)
        {
        }
    }
}