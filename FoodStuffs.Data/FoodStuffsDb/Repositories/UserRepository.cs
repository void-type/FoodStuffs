using FoodStuffs.Data.EntityFramework.Repositories;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Interfaces.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.FoodStuffsDb.Repositories
{
    public class UserRepository : Repository<IUser, User>
    {
        public override IQueryable<IUser> Stored => Context.Set<User>()
            .Include(u => u.RecipeModifiedByUser)
            .Include(u => u.RecipeCreatedByUser);

        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}