using Core.Services.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthenticationFilter(this IServiceCollection services, IHostingEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddMvc(options => { options.Filters.Add(new AllowAnonymousFilter()); });
            }
            else
            {
                services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);
            }
        }

        public static void AddAuthorizationFromRoles(this IServiceCollection services, string name, List<AuthorizationRole> roles)
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

        public static TSettings AddSettingsSingleton<TSettings>(this IServiceCollection services, IConfiguration configuration) where TSettings : class, new()
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
            services.AddSingleton<TSettings>(settings);
            return settings;
        }

        public static void AddSqlServerDbContext<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
