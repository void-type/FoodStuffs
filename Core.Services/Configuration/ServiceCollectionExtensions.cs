using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMvcAntiforgery(this IServiceCollection services)
        {
            services.AddMvc(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            services.AddAntiforgery(options =>
                options.HeaderName = "X-CSRF-TOKEN");
        }
    }
}