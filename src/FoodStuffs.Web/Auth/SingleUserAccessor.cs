using VoidCore.Model.Auth;

namespace FoodStuffs.Web.Auth
{
    public class SingleUserAccessor : ICurrentUserAccessor
    {
        private static readonly DomainUser _singleUser = new DomainUser("SingleUser", new string[0]);

        public DomainUser User => _singleUser;
    }
}
