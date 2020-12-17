using System;
using VoidCore.Model.Auth;

namespace FoodStuffs.Web.Auth
{
    public class SingleUserAccessor : ICurrentUserAccessor
    {
        private static readonly DomainUser _singleUser = new DomainUser("SingleUser", Array.Empty<string>());

        public DomainUser User => _singleUser;
    }
}
