using Core.Data.EntityFramework.Repositories;
using FoodStuffs.Data.Models;
using FoodStuffs.Model.Interfaces.Domain;
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