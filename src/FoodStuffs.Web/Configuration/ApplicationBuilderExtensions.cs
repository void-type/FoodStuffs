using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using VoidCore.AspNet.Security;

namespace FoodStuffs.Web.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IHostEnvironment environment)
        {
            // Disable in development because Vue tools extension injects a script.
            if (!environment.IsDevelopment())
            {
                app.UseContentSecurityPolicy(options =>
                {
                    options.Defaults
                        .AllowSelf();

                    options.Styles
                        .AllowSelf()

                        // Add the inline styling from error and forbidden.
                        .AllowHash("sha256", "qZf1DVNyfsB5Tl6kG5RGHR8i3XXliXpcIMDN3VN3esQ=")

                        // Add the Vue-Progressbar hash because it applies inline styling.
                        .AllowHash("sha256", "DNQ8Cm24tOHANsjo3O93DpqGvfN0qkQZsMZIt0PmA2o=");

                    options.Images
                        .AllowSelf()

                        // Add data images for compiled image assets, such as the app logo.
                        .Allow("data:");
                });
            }

            return app.UseXFrameOptions(options => options.Deny());
        }
    }
}
