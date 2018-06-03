using FoodStuffs.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodStuffs.Services.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFoodStuffsDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FoodStuffsContext>(options =>
                options.UseSqlServer(configuration["FoodStuffsConnectionString"]));
        }
    }
}