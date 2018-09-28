using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Services;
using System.Security.Principal;

namespace FoodStuffs.Web.Services
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        public CurrentUserAccessor(IFoodStuffsData data, IPrincipal user)
        {
            // TODO: fake user
            var fakeUser = data.Users.New;
            fakeUser.Id = 1;
            fakeUser.FirstName = "Fake";
            fakeUser.LastName = "User";
            User = fakeUser;
        }

        public User User { get; }
    }
}
