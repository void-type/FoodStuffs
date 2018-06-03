using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthorizationFromSettings(this IServiceCollection services, string name, List<AuthorizationRole> roles)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(name, policy => policy.RequireRole(roles.Select(role => role.Name).ToArray()));
            });
        }

        public static void AddMvcAntiforgery(this IServiceCollection services)
        {
            services.AddMvc(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");
        }

        public static TSettings ConfigureSettings<TSettings>(this IServiceCollection services, IConfiguration configuration) where TSettings : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var settings = new TSettings();
            configuration.Bind(settings);
            services.AddSingleton(settings);
            return settings;
        }
    }
}