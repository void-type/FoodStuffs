namespace Core.Services.ClientApp
{
    public class ApplicationInfo : IApplicationInfo
    {
        public string ApplicationName { get; set; }
        public string AntiforgeryToken { get; set; }
        public string UserName { get; set; }
    }
}
