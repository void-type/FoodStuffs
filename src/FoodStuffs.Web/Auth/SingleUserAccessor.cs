using VoidCore.Model.Auth;

namespace FoodStuffs.Web.Auth;

public class SingleUserAccessor : ICurrentUserAccessor
{
    private static readonly DomainUser _singleUser = new("SingleUser", []);

    public DomainUser User => _singleUser;
}
