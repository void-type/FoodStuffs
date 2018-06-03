using Core.Services.Data;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Services.Data
{
    public class UserRepository : EfWritableRepository<User>
    {
        public override IQueryable<User> Stored => Context.Set<User>()
            .Include(u => u.RecipeModifiedByUser)
            .Include(u => u.RecipeCreatedByUser);

        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}