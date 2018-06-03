using Core.Services.Configuration;
using FoodStuffs.Services.Data;
using FoodStuffs.Services.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodStuffs.Services.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFoodStuffsDbContext(this IServiceCollection services, ApplicationSettings applicationSettings)
        {
            services.AddDbContext<FoodStuffsContext>(options => options.UseSqlServer(applicationSettings.ConnectionString));
        }
    }
}