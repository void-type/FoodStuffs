using FoodStuffs.Model.Data.Models;
using System.Linq;

namespace FoodStuffs.Model.Queries
{
    public static class UserQueries
    {
        public static User GetById(this IQueryable<User> users, int id)
        {
            return users.SingleOrDefault(u => u.Id == id);
        }
    }
}