using VoidCore.Model.Auth;

namespace FoodStuffs.Web.Auth
{
    public class SingleUserAccessor : ICurrentUserAccessor
    {
        public DomainUser User => new DomainUser("SingleUser", new string[0]);
    }
}
