using Core.Data.EntityFramework;
using FoodStuffs.Data.Models;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.Services.CustomRepositories
{
    public class UserRepository : EfWritableRepository<IUser, User>
    {
        public override IQueryable<IUser> Stored => Context.Set<User>()
            .Include(u => u.RecipeModifiedByUser)
            .Include(u => u.RecipeCreatedByUser);

        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}