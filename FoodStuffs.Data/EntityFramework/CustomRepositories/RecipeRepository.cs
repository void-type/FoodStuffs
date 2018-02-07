using Core.Data.EntityFramework;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.EntityFramework.CustomRepositories
{
    public class RecipeRepository : EfWritableRepository<Recipe>
    {
        public override IQueryable<Recipe> Stored => Context.Set<Recipe>()
            .Include(r => r.CategoryRecipes)
            .ThenInclude(cr => cr.Category)
            .Include(r => r.CreatedByUser)
            .Include(r => r.ModifiedByUser);

        public RecipeRepository(DbContext context) : base(context)
        {
        }
    }
}