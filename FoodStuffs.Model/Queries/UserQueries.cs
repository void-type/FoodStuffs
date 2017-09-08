using FoodStuffs.Model.Interfaces.Domain;
using System.Linq;

namespace FoodStuffs.Model.Queries
{
    public static class UserQueries
    {
        public static IUser GetById(this IQueryable<IUser> users, int id)
        {
            return users.SingleOrDefault(u => u.Id == id);
        }
    }
}