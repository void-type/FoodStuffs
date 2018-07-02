using System.Linq;

namespace Core.Services.ClientApp
{
    public class UserNameFormatter : IUserNameFormatter
    {
        public string FromAdLogin(string adLogin)
        {
            return adLogin?.Split("\\").LastOrDefault() ?? "Unknown";
        }
    }
}
