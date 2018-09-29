using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Services;
using Microsoft.AspNetCore.Http;

namespace FoodStuffs.Web.Services
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        public CurrentUserAccessor(IFoodStuffsData data, IHttpContextAccessor httpContextAccessor)
        {
            // TODO: fake user, this should get credentials and pull the user from the db
            var fakeUser = data.Users.New;
            fakeUser.Id = 1;
            fakeUser.FirstName = "Fake";
            fakeUser.LastName = "User";
            User = fakeUser;
        }

        public User User { get; }
    }
}
