namespace Core.Services.ClientApp
{
    public interface IApplicationInfo
    {
        string ApplicationName { get; set; }
        string AntiforgeryToken { get; set; }
        string UserName { get; set; }
    }
}