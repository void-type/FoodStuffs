using Core.Data.EntityFramework;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.EntityFramework.CustomRepositories
{
    public class UserRepository : EfWritableRepository<User>
    {
        public override IQueryable<User> Stored => Context.Set<User>()
            .Include(u => u.RecipesModifiedByUser)
            .Include(u => u.RecipesCreatedByUser);

        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}