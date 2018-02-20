using Core.Data.EntityFramework;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.Service.CustomRepositories
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