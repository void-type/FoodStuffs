namespace Core.Services.Configuration
{
    public interface IApplicationSettings
    {
        string Name { get; set; }
        string ConnectionString { get; set; }
    }
}