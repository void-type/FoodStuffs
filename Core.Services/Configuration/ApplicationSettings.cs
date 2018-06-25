namespace Core.Services.Configuration
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }
}