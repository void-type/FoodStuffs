using VoidCore.AspNet.Security;

namespace FoodStuffs.Web.Startup;

public static class SecurityStartupExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IHostEnvironment env, IConfiguration config)
    {
        var vueDevServerEnabled = env.IsDevelopment() && config.GetSection("VueDevServer").GetValue("Enabled", true);
        var vueDevServerHost = config.GetSection("VueDevServer").GetValue("Host", string.Empty);

        app.UseContentSecurityPolicy(options =>
        {
            options.BaseUri
                .AllowSelf();

            options.FrameAncestors
                .AllowNone();

            options.DefaultSources
                .AllowSelf();

            options.ObjectSources
                .AllowNone();

            options.ImageSources
                .AllowSelf()
                // Bootstrap and other webpack'd assets are loaded from inline data.
                .Allow("data:");

            options.ScriptSources
                .AllowSelf();

            options.StyleSources
                .AllowSelf();

            options.FrameAncestors
                .AllowNone();

            if (vueDevServerEnabled)
            {
                // In development we need to allow unsafe eval of scripts for Vue's runtime compiler.
                options.ScriptSources
                    // Vite dev server
                    .Allow("https://" + vueDevServerHost)
                    .AllowUnsafeInline()
                    .AllowUnsafeEval();

                options.StyleSources
                    // Vite dev server
                    .Allow("https://" + vueDevServerHost)
                    .AllowUnsafeInline();

                options.ImageSources
                    .Allow("https://" + vueDevServerHost);

                // .NET and Vite hot reloading use web sockets
                options.ConnectSources
                    .AllowSelf()
                    // Vite dev server
                    .Allow("https://" + vueDevServerHost)
                    .Allow("ws:")
                    .Allow("wss:");
            }
            else
            {
                options.ScriptSources
                    // Add the Swagger UI scripts
                    .AllowNonce();

                options.StyleSources
                    // Add the Swagger UI style
                    .AllowNonce();
            }
        });

        app.UseRecommendedSecurityHeaders();

        return app;
    }
}
