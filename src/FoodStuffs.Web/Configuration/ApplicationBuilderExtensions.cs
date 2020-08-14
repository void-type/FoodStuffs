using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using VoidCore.AspNet.Security;

namespace FoodStuffs.Web.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IHostEnvironment environment)
        {
            app.UseContentSecurityPolicy(options =>
            {
                options.BaseUri
                    .AllowSelf();

                options.FrameAncestors
                    .AllowNone();

                options.Defaults
                    .AllowSelf();

                options.Objects
                    .AllowNone();

                options.Images
                    .AllowSelf()

                    // Bootstrap and other webpacked assets are loaded from inline data.
                    .Allow("data:");

                if (!environment.IsDevelopment())
                {
                    // In production we will supply hashes for unsafe styles
                    options.Styles
                        .AllowSelf()

                        // Add the Vue-Progressbar hash because it applies inline styling.
                        .AllowHash("sha256", "DNQ8Cm24tOHANsjo3O93DpqGvfN0qkQZsMZIt0PmA2o=");
                }
                else
                {
                    // In development we need to allow unsafe eval of scripts for Vue's runtime compiler.
                    options.Scripts
                        .AllowSelf()
                        .AllowUnsafeEval();

                    options.Styles
                        .AllowSelf()
                        .AllowUnsafeInline();
                }
            });

            app.UseXContentTypeOptionsNoSniff();

            return app.UseXFrameOptions(options => options.Deny());
        }
    }
}
