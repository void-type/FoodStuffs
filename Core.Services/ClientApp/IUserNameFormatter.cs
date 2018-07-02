namespace Core.Services.ClientApp
{
    public interface IUserNameFormatter
    {
        string FromAdLogin(string adLogin);
    }
}
