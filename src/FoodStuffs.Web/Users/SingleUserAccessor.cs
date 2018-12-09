using VoidCore.Model.Users;

namespace FoodStuffs.Web.Users
{
    public class SingleUserAccessor : ICurrentUserAccessor
    {
        public DomainUser User => new DomainUser("SingleUser", new string[0]);
    }
}
