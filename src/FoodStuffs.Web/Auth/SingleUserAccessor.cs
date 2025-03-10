using VoidCore.Model.Auth;

namespace FoodStuffs.Web.Auth;

public class SingleUserAccessor : ICurrentUserAccessor
{
    private static readonly DomainUser _singleUser = new("SingleUser", []);

    public async Task<DomainUser> GetUser()
    {
        return await Task.FromResult(_singleUser);
    }
}
