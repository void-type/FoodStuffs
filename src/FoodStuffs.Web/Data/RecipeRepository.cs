using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using VoidCore.AspNet.Data;

namespace FoodStuffs.Web.Data
{
    public class RecipeRepository : EfWritableRepository<Recipe>
    {
        public override IQueryable<Recipe> Stored => Context.Set<Recipe>()
        .Include(r => r.CategoryRecipe)
        .ThenInclude(cr => cr.Category);

        public RecipeRepository(DbContext context) : base(context) { }
    }
}
